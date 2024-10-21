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
    //Animator anim;

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
    }

    public void Patrol()
    {
        // Calculate distance to current waypoint
        float distanceToWaypoint = Vector2.Distance(transform.position, currentPoint.position);

        switch (myEnemy)
        {
            case EnemyType.Fish:
                Move();
                break;

            case EnemyType.Frog:
                FrogMove();
                break;

            case EnemyType.Spider:
                Move();
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
                Move();

                break;
            case EnemyType.Frog:
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
                    rb.constraints = RigidbodyConstraints2D.FreezePosition;
                    StartCoroutine(enemyAttack.FishAttack());
                    rb.constraints = RigidbodyConstraints2D.None;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    break;

                case EnemyType.Frog:
                    if (IsGrounded())
                    {
                        rb.constraints = RigidbodyConstraints2D.FreezePosition;
                        StartCoroutine(enemyAttack.FrogAttack());
                        rb.constraints = RigidbodyConstraints2D.None;
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                    break;
            }
    }

    public void ChangeSpeed(float _speed)
    {
        mySpeed = _speed;
    }

    private bool IsGrounded()
    {
        //Checking if our player is colliding with the ground.
        return Physics2D.BoxCast(enemyCollider.bounds.center, enemyCollider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    private void Move()
    {
        // Move towards the current waypoint
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
        rb.MovePosition(targetPosition);

        // Determine the direction of movement
        Vector2 movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        // Flip the sprite based on movement direction
        UpdateSpriteAndCollider(movementDirection);
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
            Vector2 newOffset = new Vector2(movementDirection.x > 0 ? 1.0f : -1.0f, 0f);
            boxCollider.offset = newOffset;
        }
    }
}
