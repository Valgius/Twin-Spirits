using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;

    private float movement = 0f;
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;


    [SerializeField] private float swimForce = 0f;
    [SerializeField] private float swimSpeed = 0f;
    [SerializeField] private float rotationSpeed = 0f;
    [SerializeField] private float waterDrag = 0f;
    [SerializeField] private float buoyancyForce = 0f;
    [SerializeField] public float maxBuoyancyVelocity = 0f;

    private bool doubleJump;

    public bool isLeaf;
    public bool isSwimming = false;

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
        //Moves the Player Horizontal
        movement = Input.GetAxisRaw("Horizontal");
        playerRb.velocity = new Vector2(movement * moveSpeed, playerRb.velocity.y);

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
            //playerRb.gravityScale = 1; // Enable gravity again
        }
    }

        private bool IsGrounded()
    {
        //Checking if our player is colliding with the ground.
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }
}