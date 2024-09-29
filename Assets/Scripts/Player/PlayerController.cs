using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : GameBehaviour
{
    public Animator anim;
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbLayer;

    public bool isLeaf;
    public bool hasLeafOrb;
    public bool hasSeaOrb;

    private float movement = 0f;
    private bool doubleJump;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;

    [Header("- Dash -")]
    public bool isFacingRight;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 0f;
    [SerializeField] private float dashingTime = 0f;
    [SerializeField] private float dashingCooldown = 0f;
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("- Swim -")]
    [SerializeField] private float swimSpeed = 0f;
    [SerializeField] private float maxSwimSpeed = 0f;
    [SerializeField] private float waterDrag = 0f;
    [SerializeField] private float buoyancyForce = 0f;
    [SerializeField] public float maxBuoyancyVelocity = 0f;

    [SerializeField] private float breathTimer = 0;
    [SerializeField] private float maxBreathTimer = 0;
    [SerializeField] private Image breathFill;
    [SerializeField] private GameObject breathPanel;
    [SerializeField] private float swimmingStateCooldown = 0.5f;
    private float swimmingStateTimer = 0f;
    public bool isSwimming = false;

    [Header("- Climb -")]
    public float climbSpeed = 0f;                      
    public float wallSlideSpeed = 0f;       
    public float wallJumpForce = 0f;       
    public float wallJumpHorizontalForce = 0f;

    public bool isClimbing = false;
    private bool isTouchingWall = false;
    private bool isWallSliding = false;
    private bool canWallJump = false;
    private Vector2 wallNormal = Vector2.zero;

    private enum MovementState { idle, running, jumping, falling, swimming, climbing}

    // Start is called before the first frame update
    void Start()
    {
        swimSpeed = maxSwimSpeed;
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        this.gameObject.GetComponent<PlayerRespawn>();
        breathTimer = maxBreathTimer;
        breathPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
            return;

        Movement();
        Jumping();
        Dashing();
        
        ClimbingAndWallJumping();
        UpdateBreathBar();

        if (IsGrounded())
        {
            anim.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        Swimming();
        SpriteFlipping();
        WallDetection();
        if (isLeaf)
        {
            ClimbingAndWallSliding();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Orb")
        {

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters water
        if (other.CompareTag("Water"))
        {
            EnterWater();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits water
        if (other.CompareTag("Water"))
        {
            ExitWater();
        }
    }

    private bool IsGrounded()
    {
        //Checking if our player is colliding with the ground.
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    private void Movement()
    {
        //Moves the Player Horizontal
        movement = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(movement * moveSpeed, playerRb.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(movement));
    }

    private IEnumerator Dash()
    {
        maxSwimSpeed = dashingPower;
        anim.SetBool("isDashing", true);
        canDash = false;
        isDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0;
        playerRb.velocity = new Vector3(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        maxSwimSpeed = swimSpeed;
        trailRenderer.emitting = false;
        playerRb.gravityScale = originalGravity;
        isDashing = false;
        anim.SetBool("isDashing", false);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Jumping()
    {
        //Allows the player to jump
        if (Input.GetButtonDown("Jump"))
        {
            if (doubleJump && isLeaf && hasLeafOrb)
            {
                //anim.SetBool("isJumping", false);
                playerRb.velocity = new Vector2(0, 0);
                Jump();
                doubleJump = false;
            }
            else
            {
                if (IsGrounded())
                {
                    Jump();
                    doubleJump = true;

                }
            }
        }
    }

    private void Jump()
    {
        playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        anim.SetBool("isJumping", true);
    }

    private void Dashing()
    {
        //Allows the player to Dash
        if (Input.GetButtonDown("Dash") && canDash && !isLeaf && hasSeaOrb)
        {
            StartCoroutine(Dash());
        }
    }

    private void Swimming()
    {
        //Check if player is in water
        if (swimmingStateTimer > 0)
        {
            swimmingStateTimer -= Time.deltaTime;
        }
        else
        {
            if (isSwimming)
            {
                if (isLeaf == false)
                {
                    Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
                    playerRb.AddForce(moveDirection * swimSpeed);
                    LimitSwimmingSpeed();
                    BreathTimer();
                    ApplyWaterDragAndBuoyancy();
                }
            }
        }
    }

    private void LimitSwimmingSpeed()
    {
        // Limit the player's maximum speed
        if (playerRb.velocity.magnitude > maxSwimSpeed)
            playerRb.velocity = playerRb.velocity.normalized * maxSwimSpeed;
    }

    private void BreathTimer()
    {
        //Enables Breath Countdown
        if (breathTimer > 0)
        {
            breathTimer -= Time.deltaTime;
        }
        else
            this.gameObject.GetComponent<PlayerRespawn>().Respawn();
    }

    private void ApplyWaterDragAndBuoyancy()
    {
        // Apply drag when swimming
        if (isSwimming)
        {
            playerRb.drag = waterDrag;

            //Apply buoyancy force to counteract gravity
            if (playerRb.velocity.y < maxBuoyancyVelocity)
            {
                playerRb.AddForce(Vector2.up * buoyancyForce, ForceMode2D.Force);
            }
        }
        else
        {
            playerRb.drag = 0f;
        }
    }

    private void EnterWater()
    {
        isSwimming = true;
        anim.SetBool("isSwimming", true);
        anim.SetBool("isJumping", false);
        breathPanel.SetActive(true);
        swimmingStateTimer = swimmingStateCooldown;
        playerRb.gravityScale = 0.5f;
    }

    private void ExitWater()
    {
        isSwimming = false;
        anim.SetBool("isSwimming", false);
        anim.SetBool("isJumping", true);
        breathTimer = maxBreathTimer;
        breathPanel.SetActive(false);
        swimmingStateTimer = swimmingStateCooldown;
        playerRb.AddForce(Vector2.up * 15, ForceMode2D.Force);
        playerRb.gravityScale = 1;
    }

    public void UpdateBreathBar()
    {
        breathFill.fillAmount = MapTo01(breathTimer, 0, maxBreathTimer);
    }

    private void ClimbingAndWallJumping()
    {
        //Allows the player to climb and Wall Jump
        if (isLeaf == true)
        {
            // Check if player can wall jump
            if (isTouchingWall && !isClimbing)
            {
                canWallJump = true;
            }
            else
            {
                canWallJump = false;
            }

            // Climb up or down when climbing
            if (isClimbing)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, Input.GetAxis("Vertical") * climbSpeed); ;
            }

            // Wall sliding
            if (isWallSliding)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -wallSlideSpeed, float.MaxValue));
            }

            // Wall jump
            if (canWallJump && Input.GetButtonDown("Jump"))
            {
                playerRb.velocity = new Vector2(-wallNormal.x * wallJumpHorizontalForce, wallJumpForce);
                isClimbing = false;
                isWallSliding = false;
                canWallJump = false;
            }
        }
    }

    private void ClimbingAndWallSliding()
    {
        // Toggle climbing state based on input and wall detection
        if (isTouchingWall && !isClimbing && Input.GetAxis("Vertical") != 0)
        {
            isClimbing = true;
            playerRb.gravityScale = 0f;
            playerRb.velocity = Vector2.zero;
            anim.SetBool("isClimbing", true);
        }
        else if (!isTouchingWall || Input.GetAxis("Vertical") == 0)
        {
            isClimbing = false;
            playerRb.gravityScale = 1f;
            anim.SetBool("isClimbing", false);
        }

        // Determine if the player is climbing or wall sliding
        if (isTouchingWall && !isClimbing && playerRb.velocity.y <= 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallDetection()
    {
        // Check for wall detection using raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.6f, LayerMask.GetMask("Climb"));

        if (hit.collider != null)
        {
            isTouchingWall = true;
            wallNormal = hit.normal;
        }
        else
        {
            isTouchingWall = false;
            wallNormal = Vector2.zero;
        }
    }

    private void SpriteFlipping()
    {
        float moveFactor = movement * Time.fixedDeltaTime;

        // Flip the sprite according to movement direction...
        if (moveFactor > 0 && !isFacingRight) FlipSprite();
        else if (moveFactor < 0 && isFacingRight) FlipSprite();
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
    }

    public void ToggleHasLeafOrb()
    {
        hasLeafOrb = !hasLeafOrb;
    }

    public void ToggleHasSeaOrb()
    {
        hasSeaOrb = !hasSeaOrb;
    }
}