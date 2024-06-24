using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : Singleton<PlayerController>
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbLayer;

    public bool isLeaf;

    private float movement = 0f;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;
    private bool doubleJump;

    [Header("Dash")]
    public bool isFacingRight;
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 0f;
    [SerializeField] private float dashingTime = 0f;
    [SerializeField] private float dashingCooldown = 0f;
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("Swim")]
    [SerializeField] private float swimForce = 0f;
    [SerializeField] private float swimSpeed = 0f;
    [SerializeField] private float rotationSpeed = 0f;
    [SerializeField] private float waterDrag = 0f;
    [SerializeField] private float buoyancyForce = 0f;
    [SerializeField] public float maxBuoyancyVelocity = 0f;
    public bool isSwimming = false;

    [Header("Climb")]
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
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
            return;

        //Moves the Player Horizontal
        movement = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(movement * moveSpeed, playerRb.velocity.y);
        float verticalInput = Input.GetAxis("Vertical");
        bool jumpInput = Input.GetKeyDown(KeyCode.Space);

        /*if (Input.GetKeyDown(KeyCode.A))
        {
            isFacingRight = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isFacingRight = true;
        }*/

        //Allows the player to jump
        if (Input.GetButtonDown("Jump"))
        {
            if (doubleJump == true)
            {
                playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                doubleJump = false;
            }
            else
            {
                if (IsGrounded())
                {
                    playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    doubleJump = true;

                }
            }
        }

        if (Input.GetButtonDown("Dash")  && canDash)
        {
            if (isFacingRight == true)
            {
                StartCoroutine(DashRight());
            }
            else
            {
                StartCoroutine(DashLeft());
            }
        }

        //Check if player is in water
        if (isSwimming)
        {
            Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            playerRb.AddForce(moveDirection * swimForce);

            // Limit the player's maximum speed
            if (playerRb.velocity.magnitude > swimSpeed)
            {
                playerRb.velocity = playerRb.velocity.normalized * swimSpeed;
            }

            // Rotate the player towards the movement direction
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Apply drag when swimming
            if (isSwimming)
            {
                playerRb.drag = waterDrag;

                // Apply buoyancy force to counteract gravity
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
                playerRb.velocity = new Vector2(playerRb.velocity.x, verticalInput * climbSpeed);
            }

            // Wall sliding
            if (isWallSliding)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -wallSlideSpeed, float.MaxValue));
            }

            // Wall jump
            if (canWallJump && jumpInput)
            {
                playerRb.velocity = new Vector2(-wallNormal.x * wallJumpHorizontalForce, wallJumpForce);
                isClimbing = false;
                isWallSliding = false;
                canWallJump = false;
            }
        }
    }

    void FixedUpdate()
    {
        float moveFactor = movement * Time.fixedDeltaTime;

        // Flip the sprite according to movement direction...
        if (moveFactor > 0 && !isFacingRight) FlipSprite();
        else if (moveFactor < 0 && isFacingRight) FlipSprite();

        if (isDashing)
            return;

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

        // Determine if the player is climbing or wall sliding
        if (isTouchingWall && !isClimbing && playerRb.velocity.y <= 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        // Toggle climbing state based on input and wall detection
        if (isTouchingWall && !isClimbing && Input.GetAxis("Vertical") != 0)
        {
            isClimbing = true;
            playerRb.gravityScale = 0f;
            playerRb.velocity = Vector2.zero;
        }
        else if (!isTouchingWall || Input.GetAxis("Vertical") == 0)
        {
            isClimbing = false;
            playerRb.gravityScale = 1f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters water
        if (other.CompareTag("Water"))
            if (isLeaf == false)
            {
                isSwimming = true;
                //playerRb.gravityScale = 0; // Disable gravity while swimming
            }
        else
            {
                print("Respwan");
            }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits water
        if (other.CompareTag("Water"))
        {
            isSwimming = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            //playerRb.gravityScale = 1; // Enable gravity again
        }
    }

    private bool IsGrounded()
    {
        //Checking if our player is colliding with the ground.
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    private IEnumerator DashRight()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0;
        playerRb.velocity = new Vector3(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        playerRb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private IEnumerator DashLeft()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0;
        playerRb.velocity = new Vector3(transform.localScale.x * -dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        playerRb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
    }
}