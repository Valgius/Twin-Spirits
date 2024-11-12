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
    private PlayerHealth playerHealth;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask climbLayer;
    public GameObject pausePanel;
    CheckpointManager manager;

    public bool isLeaf;
    public bool hasLeafOrb;
    public bool hasSeaOrb;

    [Header("- Movement -")]
    [SerializeField] private float movement = 0f;
    [SerializeField] private bool doubleJump;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private float doubleJumpForce = 1f;
    [SerializeField] private float gravity = 1f;
    public bool isGrounded;
    public float stepRate = 0.5f;
    float stepCooldown;

    [Header("- Dash -")]
    public bool isFacingRight;
    [SerializeField] private bool canDash = true;
    public bool isDashing;
    [SerializeField] private float dashingPower = 0f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashPenalty = 2f;

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
    [SerializeField] private float breathCooldown = 5f;
    private bool firstSwim = true;

    [Header("- Climb -")]
    public float climbSpeed = 0f;                      
    public float wallSlideSpeed = 0f;       
    public float wallJumpForce = 0f;       
    public float wallJumpHorizontalForce = 0f;
    [SerializeField] private float wallJumpTimer = 0.5f;

    public bool isClimbing = false;
    [SerializeField] private bool isTouchingWall = false;
    private bool isWallSliding = false;
    [SerializeField] private bool canWallJump = false;
    private Vector2 wallNormal = Vector2.zero;

    [Header("Knockback")]
    [SerializeField] private bool isKnockback = false;
    [SerializeField] private float knockbackTimer = 0f;
    [SerializeField] private float force = 30f;

    [Header("Death")]
    FadeOut fadeOut;

    private enum MovementState { idle, running, jumping, falling, swimming, climbing}

    
    void Start()
    {
        swimSpeed = maxSwimSpeed;
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        this.gameObject.GetComponent<PlayerRespawn>();
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
        breathTimer = maxBreathTimer;
        ToggleBreath(false);
        fadeOut = FindObjectOfType<FadeOut>();
        manager = FindObjectOfType<CheckpointManager>();
    }

    
    void Update()
    {
        //When the player dies, freeze player and stop jump animation, if alive, enable colliders and set constraints.
        if (fadeOut.playerDie)
        {
            playerRb.velocity = new Vector2(0f, 0f);
            anim.SetBool("isJumping", false);
            playerRb.GetComponent<BoxCollider2D>().enabled = false;
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            playerRb.GetComponent<BoxCollider2D>().enabled = true;
            playerRb.constraints = RigidbodyConstraints2D.None;
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
            
        //If the player is dashing, dying or if the player pauses, dont run anything after.
        if (isDashing || pausePanel.activeSelf || fadeOut.playerDie || manager.isPaused)
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

        if(knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
        }
        if(breathCooldown > 0)
        {
            StartCoroutine(RefreshBreath());
        }

        //DEV TEST KEY FOR ORBS.
        if (Input.GetKeyDown(KeyCode.O))
        {
            hasSeaOrb = true;
            hasLeafOrb = true;
        }

        //Set the yVelocity in the Animator
        anim.SetFloat("yVelocity", playerRb.velocity.y);
    }

    void FixedUpdate()
    {
        //If player is dashing, dying or pausing the game, ignore the rest of code.
        if (isDashing || pausePanel.activeSelf || fadeOut.playerDie || manager.isPaused)
            return;

        Swimming();
        SpriteFlipping();

        //If wallJumpTimer is zero, run WallDetection function.
        if(wallJumpTimer <= 0)
        {
            WallDetection();
        }
        
        //If the player is leaf, run Climbing function.
        if (isLeaf)
        {
            ClimbingAndWallSliding();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If touching ground, isGrounded is true.
        if (collision.gameObject.CompareTag("Ground") && !isSwimming)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //When the player is touching an enemy and the hit cooldown is zero, run the knockback script.
        if (collision.gameObject.CompareTag("Enemy") && playerHealth.hitCooldown <= 0)
        {
            //Assign knockback values
            Vector2 knockback = new Vector2(transform.position.x - collision.transform.position.x, transform.position.y - collision.transform.position.y);
            Vector2 knockbackForce = knockback * force;

            playerRb.velocity = knockbackForce;
            isKnockback = true;
            knockbackTimer = 0.5f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //When the player leaves the ground, isGrounded is false.
        if (collision.gameObject.CompareTag("Ground") && !isSwimming)
        {
            isGrounded = false;
            anim.SetFloat("Speed", 0f);
            anim.SetBool("isJumping", true);
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
        {
            flow = other.gameObject.GetComponent<WaterFlow>();
            DashEnd();
        }
            

        //When the player is hit by GeyserProjectile, start knockback with timer.
        if (other.gameObject.CompareTag("GeyserProjectile") && isDashing == false)
        {
            //Assign knockback values
            Vector2 knockback = new Vector2(transform.position.x - other.transform.position.x, 0f);
            Vector2 knockbackUp = new Vector2(0f, transform.position.y - other.transform.position.y);
            Vector2 knockbackForce = knockback * force;
            //Depending on projectile direction, knockback player in direction of knockback.
            switch (other.gameObject.GetComponentInParent<WaterGeyser>().direction) 
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
            knockbackTimer = 0.5f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Consult OnCollisionStay2D for details.
        if (other.gameObject.CompareTag("Enemy") && playerHealth.hitCooldown <= 0)
        {
            //Assign knockback values
            Vector2 knockback = new Vector2(transform.position.x - other.transform.position.x, transform.position.y - other.transform.position.y);
            Vector2 knockbackForce = knockback * force;

            playerRb.velocity = knockbackForce;
            isKnockback = true;
            knockbackTimer = 0.5f;
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
        if(isKnockback || !isLeaf && isSwimming)
            return;

        //Moves the Player Horizontal
        movement = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(movement * moveSpeed, playerRb.velocity.y);
        if (isGrounded)
        {
            anim.SetFloat("Speed", Mathf.Abs(movement));
            anim.SetBool("isGrounded", true);
        }

        //if(Input.GetAxis("Horizontal") == +1)
        //{
        //    playerRb.velocity = new Vector2(moveSpeed, playerRb.velocity.y);
        //}
        //if(Input.GetAxis("Horizontal") == -1)
        //{
        //    playerRb.velocity = new Vector2(-moveSpeed, playerRb.velocity.y);
        //}

        //Footstep Audio
        stepCooldown -= Time.deltaTime;
        if (stepCooldown < 0 && isGrounded && (movement != 0))
        {
            stepCooldown = stepRate;
            _AM.PlaySFX("Player Run");
        }
    }

    /// <summary>
    /// Underwater Dash function.
    /// </summary>
    /// <returns></returns>
    private void Dash()
    {
        //Enables the dashing animation, changes can dash to false so the player can't dash while dashing and is dashing to true.
        canDash = false; 
        isDashing = true; 
        anim.SetBool("isDashing", true); 
        //Set new player velocity to speed the player up
        playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y) * dashingPower;
        breathTimer -= dashPenalty;
        
        _AM.PlaySFX("Player Dash");
    }

    /// <summary>
    /// Dash end function to be called at animation end, or whenever we need dashing to stop.
    /// </summary>
    public void DashEnd()
    {
        isDashing = false;
        canDash = true;
        anim.SetBool("isDashing", false);
    }

    private void Jumping()
    {
        //Allows the player to jump
        if (Input.GetButtonDown("Jump"))
        {
            if (doubleJump && isLeaf && hasLeafOrb && !IsGrounded())
            {
                playerRb.velocity = new Vector2(0, 0);
                DoubleJump();
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

    /// <summary>
    /// Applies force upwards, plays jumping animation and sets isGrounded off.
    /// </summary>
    private void Jump()
    {
        playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        anim.SetBool("isJumping", true);
        anim.SetBool("isGrounded", false);
        isGrounded = false;
        _AM.PlaySFX("Jump");
    }

    /// <summary>
    /// Same as Jump function except using doubleJumpForce.
    /// </summary>
    private void DoubleJump()
    {
        playerRb.AddForce(new Vector2(0, doubleJumpForce), ForceMode2D.Impulse);
        anim.SetBool("isJumping", true);
        _AM.PlaySFX("Jump");
    }

    private void Dashing()
    {
        //Allows the player to Dash
        if (Input.GetButtonDown("Dash") && canDash && !isLeaf && hasSeaOrb && isSwimming && !isKnockback)
        {
             Dash();
        }
    }

    /// <summary>
    /// Determines swimming state functions.
    /// </summary>
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
            if (isLeaf)
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
                movement = Input.GetAxis("Horizontal");
                anim.SetFloat("Speed", Mathf.Abs(movement));
            }

            //When holding Space, the player will swim upwards.
            if (Input.GetButton("Jump")) 
            {
                playerRb.velocity = new Vector2(moveDirection.x, 1) * swimSpeedUp;
            }
            LimitSwimmingSpeed();
            BreathTimer();
            ApplyWaterDragAndBuoyancy();

            //Swim Audio
            stepCooldown -= Time.deltaTime;
            if (stepCooldown < 0 && (movement != 0))
            {
                stepCooldown = stepRate;
                _AM.PlaySFX("Player Swim");
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
        if (firstSwim)
        {
            Tutorial tutorial = FindObjectOfType<Tutorial>();
            tutorial.SwimTutorial();
            firstSwim = false;
        }
        isGrounded = false;
        isSwimming = true;
        if(breathCooldown > 0)
        {
            breathCooldown = -1;
        }
        anim.SetBool("isSwimming", true);
        anim.SetBool("isJumping", false);
        ToggleBreath(true);
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
        breathCooldown = 5f;
        breathPanel.SetActive(false);
        breathTimer = maxBreathTimer;
        ToggleBreath(false);
        swimmingStateTimer = swimmingStateCooldown;
        playerRb.gravityScale = gravity;
        playerRb.drag = 0f;
        _AM.PlaySFX("Player Dive");
        DashEnd();
    }

    IEnumerator RefreshBreath()
    {
        
        if (breathTimer > 0)
        {
            breathCooldown -= Time.deltaTime;
        }
        if(breathCooldown <= 0)
        {
            breathTimer = maxBreathTimer;
        }

        yield return null;
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
        if (isTouchingWall)
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
            anim.SetBool("isGrounded", false);
        }

        // Wall sliding
        if (isWallSliding)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }

        // Wall jump
        if (canWallJump && Input.GetButtonDown("Jump") && wallJumpTimer <=0)
        {
            if (hasLeafOrb)
            {
                doubleJump = true;
            }
            playerRb.velocity = new Vector2(-wallNormal.x * wallJumpHorizontalForce, wallJumpForce);
            isClimbing = false;
            isWallSliding = false;
            canWallJump = false;
            isTouchingWall = false;
            anim.SetBool("isJumping", true);
            wallJumpTimer = 0.5f;
            _AM.PlaySFX("Jump");
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

            //Climbing Audio
            stepCooldown -= Time.deltaTime;
            stepCooldown = stepRate;
            _AM.PlaySFX("Player Climb");
        }
        else if (!isTouchingWall || Input.GetAxis("Vertical") == 0)
        {
            isClimbing = false;
            playerRb.gravityScale = gravity;
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
        if (!isLeaf)
            return;

        // Check for wall detection using raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, 0.6f, LayerMask.GetMask("Climb"));

        if (hit.collider != null)
        {
            isTouchingWall = true;
            anim.SetBool("isClimbing", true);
            wallNormal = hit.normal;
            if(wallJumpTimer <= 0)
            {
                anim.SetBool("isJumping", false);
            }
            
        }
        else
        {
            if (!isGrounded)
            {
                anim.SetBool("isJumping", true);
                anim.SetFloat("Speed", 0f);
            }
            isTouchingWall = false;
            anim.SetBool("isClimbing", false);
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

    public void ToggleBreath(bool active)
    {
        if (!isLeaf)
        {
            breathPanel.SetActive(active);
        }
    }
}