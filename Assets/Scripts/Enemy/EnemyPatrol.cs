using System.Collections;
using UnityEngine;

public class EnemyPatrol : GameBehaviour
{
    public PatrolType myPatrol;
    public EnemyType myEnemy;
    public EnemyAttack enemyAttack;

    public GameObject pointA;
    public GameObject pointB;

    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D enemyCollider;
    public Transform currentPoint;
    private Transform playerSea;
    private Transform playerLeaf;
    public Transform closestPlayer;

    SpriteRenderer spriteRenderer;
    public Animator enemyAnim;

    [Header("AI")]
    public float baseSpeed = 1f;
    public float mySpeed = 1f;
    public float chaseSpeed = 1f;
    public float jumpHeight = 1f;
    private Vector2 jumpDirection;
    public float jumpCooldown = 1f;
    private bool canJump = true;
    public float attackDistance = 0.1f;
    private float detectCountdown = 5f;
    public float detectTime = 5f;
    public float detectDistance = 10f;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        playerLeaf = GameObject.Find("PlayerLeaf").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPoint = pointB.transform;
        mySpeed = baseSpeed;
        detectCountdown = detectTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (myPatrol == PatrolType.Die)
            return; //cancels anything after this line

        //Always get the distance between the players and this object and assign the closest player.
        float distToSea = Vector3.Distance(transform.position, playerSea.transform.position);
        float distToLeaf = Vector3.Distance(transform.position, playerLeaf.transform.position);

        closestPlayer = (distToLeaf < distToSea) ? playerLeaf : playerSea;

        float distToClosest = Vector3.Distance(transform.position, closestPlayer.transform.position);
        if (distToClosest <= detectDistance && myPatrol != PatrolType.Attack)
        {
            if (myPatrol != PatrolType.Chase)
            {
                myPatrol = PatrolType.Detect;
            }
        }

        //Switching patrol states logic
        switch (myPatrol)
        {
            case PatrolType.Patrol:
                Patrol();
                break;

            case PatrolType.Detect:
                Detect(distToClosest);
                break;

            case PatrolType.Chase:
                Chase(distToClosest);
                break;
        }

        switch (myEnemy)
        {
            case EnemyType.Frog:
                //Set the yVelocity in the Animator
                enemyAnim.SetFloat("yVelocity", rb.velocity.y);
                if (isGrounded)
                {
                    enemyAnim.SetBool("isJumping", false);
                }
                break;
        }
    }

    public void Patrol()
    {
        // Calculate distance to current waypoint
        float distanceToWaypoint = Vector2.Distance(transform.position, currentPoint.position);

        switch (myEnemy)
        {
            case EnemyType.Fish:
                FishMove();
                break;

            case EnemyType.Frog:
                FrogMove();
                break;

            case EnemyType.Spider:
                SpiderMove();
                break;
        }

        if (distanceToWaypoint < 1.5f)
        {
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }
    }

    private void Detect(float distToClosest)
    {
        //Stop speed
        ChangeSpeed(0);

        //Decrement our detect time
        detectCountdown -= Time.deltaTime;

        if (distToClosest <= detectDistance)
        {

            switch (myEnemy)
            {
                case EnemyType.Spider:
                    StartCoroutine(enemyAttack.SpiderAttack());
                    break;
            }
            detectCountdown = detectTime;
        }

        if (detectCountdown <= 0)
        {
            ChangeSpeed(baseSpeed);
            myPatrol = PatrolType.Patrol;
        }
    }

    private void Chase(float distToClosest)
    {
        switch (myEnemy)
        {
            case EnemyType.Fish:
                FishMove();

                break;
            case EnemyType.Frog:
                if (isGrounded)
                    enemyAnim.SetBool("isJumping", false);
                Wait(0.5f);
                FrogMove();
                break;
        }

        //increase the speed of which to chase the player
        ChangeSpeed(baseSpeed + chaseSpeed);

        //Check if we are close to the player, then attack         
        if (distToClosest <= attackDistance)
            switch (myEnemy)
            {
                case EnemyType.Fish:
                    FreezeConstraints();
                    StartCoroutine(enemyAttack.FishAttack());
                    UnFreezeConstraints();
                    break;

                case EnemyType.Frog:
                    if (IsGrounded())
                    {
                        FreezeConstraints();
                        StartCoroutine(enemyAttack.FrogAttack());
                        UnFreezeConstraints();
                    }
                    break;
            }
    }

    public void ChangeSpeed(float _speed)
    {
        mySpeed = _speed;
        enemyAnim.SetFloat("Speed", Mathf.Abs(mySpeed));
    }

    private bool IsGrounded()
    {
        //Checking if our player is colliding with the ground.
        return Physics2D.BoxCast(enemyCollider.bounds.center, enemyCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    private void FishMove()
    {
        // Move towards the current waypoint
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
        rb.MovePosition(targetPosition);

        // Determine the direction of movement
        Vector2 movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        // Flip the sprite based on movement direction
        UpdateSpriteAndCollider(movementDirection);
    }

    private void SpiderMove()
    {
        // Move towards the current waypoint
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);


        rb.MovePosition(targetPosition);

        RotateTowardsWaypoints();

        if (rb.rotation >= 0f) 
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
         else
        {
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }

        // Determine the direction of movement
        Vector2 movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        // Flip the sprite based on movement direction
        UpdateSpriteAndCollider(movementDirection);

        enemyAnim.SetFloat("Speed", Mathf.Abs(mySpeed));
    }

    public void FrogMove()
    {
        // Calculate the direction to the current point
        Vector2 movementDirection = (currentPoint.position - transform.position).normalized;

        if (IsGrounded() && canJump)
        {
            // Set the jump direction only if grounded
            jumpDirection = movementDirection;
            rb.velocity = new Vector2(jumpDirection.x * mySpeed, jumpHeight);
            enemyAnim.SetBool("isJumping", true);
            isGrounded = false;

            // Start the cooldown coroutine
            StartCoroutine(JumpCooldownCoroutine());

            UpdateSpriteAndCollider(movementDirection);

        }
        else if (!IsGrounded())
        {
            // While in the air, maintain the horizontal velocity
            rb.velocity = new Vector2(jumpDirection.x * mySpeed, rb.velocity.y);
        }
    }

    public void CalculateClosestPlayer()
    {
        //Always get the distance between the players and this object and assign the closest player.
        float distToSea = Vector3.Distance(transform.position, playerSea.transform.position);
        float distToLeaf = Vector3.Distance(transform.position, playerLeaf.transform.position);

        closestPlayer = (distToLeaf < distToSea) ? playerLeaf : playerSea;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointA.transform.position, 1f);
        Gizmos.DrawSphere(pointB.transform.position, 1f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);

        //Gizmos.DrawSphere(this.gameObject.transform.position ,detectDistance);
    }

    private IEnumerator JumpCooldownCoroutine()
    {
        canJump = false; // Disable jumping
        yield return new WaitForSeconds(jumpCooldown); // Wait for the cooldown duration
        canJump = true; // Enable jumping again
    }

    private void UpdateSpriteAndCollider(Vector2 movementDirection)
    {
        // Flip the sprite based on movement direction
        spriteRenderer.flipX = movementDirection.x < 0;

        // Adjust collider offset for Fish type
        if (myEnemy == EnemyType.Fish)
        {
            BoxCollider2D boxCollider = enemyAttack.fishAttackBox.GetComponent<BoxCollider2D>();
            Vector2 newOffset = new Vector2(movementDirection.x > 0 ? 1.5f : -1.5f, 0f);
            boxCollider.offset = newOffset;
        }
    }

    public void ToggleComponents(bool isActive)
    {
        if (isActive)
        {
            UnFreezeConstraints();
            spriteRenderer.enabled = true;
            enemyCollider.enabled = true;
        }
        else
        {
            FreezeConstraints();
            spriteRenderer.enabled = false;
            enemyCollider.enabled = false;
        }
    }

    private void FreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void UnFreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void RotateTowardsWaypoints()
    {
        // Get the direction vector between waypoints
        Vector2 direction = (currentPoint.position - transform.position).normalized;
        // Calculate the angle in degrees from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the rotation of the spider
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
