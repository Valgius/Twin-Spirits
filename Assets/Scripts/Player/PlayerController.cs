using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

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

    [Header("- Movement -")]
    private float movement = 0f;
    [SerializeField] private bool doubleJump;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;
    public bool isGrounded;
    //public float stepRate = 0.5f;
    //float stepCooldown;

    [Header("- Dash -")]
    public bool isFacingRight;
    private bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashingPower = 0f;
    [SerializeField] private float dashingTime = 0f;
    [SerializeField] private float dashingCooldown = 0f;
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("- Swim -")]
    [SerializeField] public float swimSpeed = 0f;
    [SerializeField] private float swimSpeedUp = 0f;
    [SerializeField] public float maxSwimSpeed = 0f;
    [SerializeField] private float waterDrag = 0f;
    [SerializeField] private float buoyancyForce = 0f;
    [SerializeField] public float maxBuoyancyVelocity = 0f;

    [SerializeField] private float breathTimer = 0;
    [SerializeField] private float maxBreathTimer = 0;
    [SerializeField] private Image breathFill;
    [SerializeField] private GameObject breathPanel;
    [SerializeField] private float swimmingStateCooldown = 0.5f;
    [SerializeField] private float swimDeceleration;
    private float swimmingStateTimer = 0f;
    public bool isSwimming = false;
    public WaterFlow flow;
    private bool isKnockback = false;
    [SerializeField] private float knockbackTimer = 0f;
    public float force = 30f;
    [SerializeField] private float dashingPushback;

    [Header("- Climb -")]
    public float climbSpeed = 0f;                      
    public float wallSlideSpeed = 0f;       
    public float wallJumpForce = 0f;       
    public float wallJumpHorizontalForce = 0f;
    [SerializeField] private float wallJumpTimer = 0.5f;

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
        Dashing();

        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
            //isGrounded = true;
        }

        //If knockbackTimer is less than or equal to 0, knockback is false, allowing movement.
        if(knockbackTimer <=0)
            isKnockback = false;

        if(wallJumpTimer > 0)
            wallJumpTimer -= Time.deltaTime;

        Jumping();

        ClimbingAndWallJumping();
        UpdateBreathBar();

        if (Input.GetKeyDown(KeyCode.O))
        {
            hasSeaOrb = true;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
            return;

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
        if (collision.gameObject.CompareTag("Ground") && !isSwimming)
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters water
        if (other.CompareTag("Water"))
        {
            EnterWater();
        }
        if (other.gameObject.GetComponent<WaterFlow>() && flow == null)
            flow = other.gameObject.GetComponent<WaterFlow>();

        //When the player is hit by GeyserProjectile, start knockback with timer.
        if (other.gameObject.CompareTag("GeyserProjectile") && isDashing == false)
        {
            //Assign knockback values
            Vector2 knockback = new Vector2(transform.position.x - other.transform.position.x, 0f);
            Vector2 knockbackUp = new Vector2(0f, transform.position.y - other.transform.position.y);
            Vector2 knockbackForce = knockback * force;
            switch (other.gameObject.GetComponentInParent<WaterGeyser>().direction) // Depending on projectile direction, knockback player in direction of knockback.
            {
                case WaterGeyser.Direction.Down:
                    playerRb.velocity = knockbackForce;
                    break;
                case WaterGeyser.Direction.Up:
                    playerRb.velocity = knockbackForce;
                    break;
                case WaterGeyser.Direction.Right: 
                    playerRb.velocity = knockbackUp * force;
                    break;
                case WaterGeyser.Direction.Left:
                    playerRb.velocity = knockbackUp * force;
                    break;
            }
            isKnockback = true;
            knockbackTimer = 1f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits water
        if (other.CompareTag("Water"))
        {
            ExitWater();
        }
        flow = null;
    }

    private bool IsGrounded()
    {
        //Checking if our player is colliding with the ground.
        bool grounded = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);

        //Debug.Log("IsGrounded: " + grounded);
        return grounded;
    }

    private void Movement()
    {
        if(isSwimming == true)
            return;
        //Moves the Player Horizontal
        movement = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(movement * moveSpeed, playerRb.velocity.y);
        anim.SetFloat("Speed", Mathf.Abs(movement));

        /*//Footstep Audio Stuff
        stepCooldown -= Time.deltaTime;
        if (stepCooldown < 0 && isGrounded && (movement != 0))
        {
            stepCooldown = stepRate;
            _AM.PlaySFX("Player Run");
        }*/
    }

    private IEnumerator Dash()
    {
        LimitSwimmingSpeed();
        //Enables the dashing animation, changes can dash to false so the player can't dash while dashing and is dashing to true.
        canDash = false; isDashing = true; anim.SetBool("isDashing", true); //_AM.PlaySFX("Player Dash");
        //Set new player velocity to speed the player up and emit a trail.
        trailRenderer.emitting = true;
        playerRb.velocity *= dashingPower; 
        //After waiting a set amount of time, reset the player back to original swimming state.
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false; anim.SetBool("isDashing", false);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Jumping()
    {
        //Allows the player to jump
        if (Input.GetButtonDown("Jump"))
        {
            if (doubleJump && isLeaf && hasLeafOrb && !IsGrounded())
            {
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
        isGrounded = false;
        _AM.PlaySFX("Jump");
    }

    private void Dashing()
    {
        //Allows the player to Dash
        if (Input.GetButtonDown("Dash") && canDash && !isLeaf && hasSeaOrb && isSwimming && !isKnockback)
        {
            StartCoroutine(Dash());
        }
    }

    private void Swimming()
    {
        if (!isSwimming)
            return;

        //Check if player is in water
        if (swimmingStateTimer > 0)
        {
            swimmingStateTimer -= Time.deltaTime;
        }
        else
        {
            if (isLeaf == true)
                return;

            //Get move direction using Input keys.
            Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            //Get Vector2 of the water flow and apply movement speed depending on direction.
            if (flow != null)
            {
                print("got flow direction: " + this.flow.GetCurrentDirection());
                moveDirection += flow.GetCurrentDirection();
            }

            //If the player isn't getting knocked back, allow movement with animations.
            if (isKnockback != true)
            {
                playerRb.velocity = moveDirection * swimSpeed;
                movement = Input.GetAxisRaw("Horizontal");
                anim.SetFloat("Speed", Mathf.Abs(movement));
            }
            else
                knockbackTimer -= Time.deltaTime;
            

            if (Input.GetKey(KeyCode.Space)) //When holding Space, the player will swim upwards.
            {
                playerRb.velocity = Vector2.up * swimSpeedUp;
            }
            LimitSwimmingSpeed();
            BreathTimer();
            ApplyWaterDragAndBuoyancy();
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
        isGrounded = false;
        isSwimming = true;
        anim.SetBool("isSwimming", true);
        anim.SetBool("isJumping", false);
        breathPanel.SetActive(true);
        swimmingStateTimer = swimmingStateCooldown;
        playerRb.gravityScale = 0.5f;
        _AM.PlaySFX("Player Dive");
    }

    private void ExitWater()
    {
        playerRb.velocity = playerRb.velocity.normalized * swimSpeedUp;
        isSwimming = false;
        anim.SetBool("isSwimming", false);
        anim.SetBool("isJumping", true);
        breathTimer = maxBreathTimer;
        breathPanel.SetActive(false);
        swimmingStateTimer = swimmingStateCooldown;
        playerRb.gravityScale = 1;
        playerRb.drag = 0f;
        _AM.PlaySFX("Player Dive");
    }

    public void UpdateBreathBar()
    {
        breathFill.fillAmount = MapTo01(breathTimer, 0, maxBreathTimer);
    }

    private void ClimbingAndWallJumping()
    {
        //Allows the player to climb and Wall Jump
        if (!isLeaf)
            return;
        
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
            playerRb.velocity = new Vector2(playerRb.velocity.x, Input.GetAxis("Vertical") * climbSpeed);
        }

        // Wall sliding
        if (isWallSliding)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        // Wall jump
        if (canWallJump && Input.GetButtonDown("Jump") && wallJumpTimer <=0)
        {
            playerRb.velocity = new Vector2(-wallNormal.x * wallJumpHorizontalForce, wallJumpForce);
            isClimbing = false;
            isWallSliding = false;
            canWallJump = false;
            anim.SetBool("isJumping", true);
            wallJumpTimer = 0.5f;
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
            if(wallJumpTimer <= 0)
            {
                anim.SetBool("isJumping", false);
            }
            
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