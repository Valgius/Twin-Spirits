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
    private Transform currentPoint;
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
    public float attackDistance = 2;
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

    private void Patrol()
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

        if (distanceToWaypoint < 1f)
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
                case EnemyType.Fish:
                    currentPoint = closestPlayer;
                    myPatrol = PatrolType.Chase;
                    break;
                case EnemyType.Frog:
                    myPatrol = PatrolType.Chase;
                    currentPoint = closestPlayer;
                    break;
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

        //If the player gets outside the detect distance, go back to the detect state.
        if (distToClosest > detectDistance)
        {
            currentPoint = pointA.transform;
            myPatrol = PatrolType.Detect;
        }

        //Check if we are close to the player, then attack         
        if (distToClosest <= attackDistance)
            switch (myEnemy)
            {
                case EnemyType.Fish:
                    StartCoroutine(enemyAttack.FishAttack());
                    break;

                case EnemyType.Frog:
                    StartCoroutine(enemyAttack.FrogAttack());
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

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
    }

    private void Move()
    {
        // Move towards current waypoint
        Vector2 point = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
        rb.MovePosition(point);

        // Determine the direction of movement
        Vector2 movementDirection = (point - (Vector2)transform.position).normalized;

        // Flip the sprite based on movement direction
        if (movementDirection.x > 0)
        {
            // Moving right
            spriteRenderer.flipX = false;
        }
        else if (movementDirection.x < 0)
        {
            // Moving left
            spriteRenderer.flipX = true;
        }
    }

    private void FrogMove()
    {
        // Calculate the direction to the current point
        Vector2 direction = (currentPoint.position - transform.position).normalized;

        // Set the velocity of the Rigidbody2D to move towards the current point
        rb.velocity = new Vector2(direction.x * mySpeed, rb.velocity.y);

        // Flip the sprite based on movement direction
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false; // Moving right
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;  // Moving left
        }

        Jump();
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
}
