using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    private Transform closestPlayer;
    SpriteRenderer spriteRenderer;

    //private Animator anim;

    [Header("AI")]
    public float baseSpeed = 1f;
    public float mySpeed = 1f;
    public float chaseSpeed = 1f;
    public float jumpForce = 1f;
    public float attackDistance = 2;
    private float detectCountdown = 5f;
    public float detectTime = 5f;
    public float detectDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        mySpeed = baseSpeed;
        detectCountdown = detectTime;
        enemyCollider = GetComponent<BoxCollider2D>();
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        playerLeaf = GameObject.Find("PlayerLeaf").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPatrol == PatrolType.Die)
            return; //cancels anything after this line

        //Always get the distance between the players and this object and assign the closest player.
        float distToSea = Vector3.Distance(transform.position, playerSea.transform.position);
        float distToLeaf = Vector3.Distance(transform.position, playerLeaf.transform.position);

        if (distToLeaf < distToSea)
        {
            closestPlayer = playerLeaf;
        }
        else
        {
            closestPlayer = playerSea;
        }

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
                // Calculate distance to current waypoint
                float distanceToWaypoint = Vector2.Distance(transform.position, currentPoint.position);

                switch (myEnemy)
                {
                    case EnemyType.Fish:
                        Move();
                        break;

                    case EnemyType.Frog:
                        Move();
                        //Jump();
                        /*
                        Vector2 frogPoint = currentPoint.position - transform.position;
                        if (currentPoint == pointB.transform)
                        {
                            rb.velocity = new Vector2(mySpeed, 0);
                            Jump();
                        }

                        else
                        {
                            rb.velocity = new Vector2(-mySpeed, 0);
                            Jump();
                        }*/
                        break;

                    case EnemyType.Spider:
                        Move();
                        break;    
                }

                // If close to current waypoint, switch to the next one
                if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointB.transform)
                {
                    currentPoint = pointA.transform;
                }

                if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointA.transform)
                {
                    currentPoint = pointB.transform;
                }
                break;
                
            case PatrolType.Detect:
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
                break;

            case PatrolType.Chase:

                switch (myEnemy)
                {
                    case EnemyType.Fish:
                        Move();

                        break;
                    case EnemyType.Frog:
                        Move();
                        //Jump();
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
                if (distToClosest<= attackDistance)
                    switch (myEnemy)
                {
                    case EnemyType.Fish:
                            StartCoroutine(enemyAttack.FishAttack());
                            break;

                    case EnemyType.Frog:
                            StartCoroutine(enemyAttack.FrogAttack());
                            break;
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

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointA.transform.position, 1f);
        Gizmos.DrawSphere(pointB.transform.position, 1f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);

        //Gizmos.DrawSphere(this.gameObject.transform.position ,detectDistance);
    }
}
