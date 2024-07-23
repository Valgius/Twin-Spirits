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

    //private Animator anim;

    [Header("AI")]
    public float baseSpeed = 1f;
    public float mySpeed = 1f;
    public float chaseSpeed = 1f;
    public float jumpForce = 1f;
    public float attackDistance = 2;
    public float detectTime = 5f;
    public float detectDistance = 10f;
    public int patrolDistance = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        mySpeed = baseSpeed;
        enemyCollider = GetComponent<BoxCollider2D>();
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        playerLeaf = GameObject.Find("PlayerLeaf").GetComponent<Transform>();
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
                        // Move towards current waypoint
                        Vector2 fishPoint = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
                        rb.MovePosition(fishPoint);
                        break;

                    case EnemyType.Frog:
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
                        }
                        break;

                    case EnemyType.Spider:
                        // Move towards current waypoint
                        Vector2 spiderPoint = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
                        rb.MovePosition(spiderPoint);
                        break;    
                }

                // If close to current waypoint, switch to the next one and flip Sprite
                if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointB.transform)
                {
                    currentPoint = pointA.transform;
                    FlipSprite();
                }

                if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointA.transform)
                {
                    currentPoint = pointB.transform;
                    FlipSprite();
                }
                break;
                
            case PatrolType.Detect:
               //Stop speed
                ChangeSpeed(0);

                //Decrement our detect time
                detectTime -= Time.deltaTime;
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
                            myPatrol = PatrolType.Attack;
                            break;
                    }
                    detectTime = 5;
                }
                
                if (detectTime <= 0)
                {
                    ChangeSpeed(baseSpeed);
                    myPatrol = PatrolType.Patrol;              
                }
                break;

            case PatrolType.Chase:

                switch (myEnemy)
                {
                    case EnemyType.Fish:
                        Vector2 fishTarget = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
                        rb.MovePosition(fishTarget);

                        break;
                    case EnemyType.Frog:
                        Vector2 frogTarget = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
                        rb.MovePosition(frogTarget);
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

                /*
                if (distToLeaf <= attackDistance)
                    switch (myEnemy)
                {
                    case EnemyType.Fish:
                            StartCoroutine(enemyAttack.FishAttack());
                            break;

                    case EnemyType.Frog:
                            StartCoroutine(enemyAttack.FrogAttack());
                            break;
                }
                */
                break;        
        }
    }

    private void FlipSprite()
    {
        Vector3 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointA.transform.position, 1f);
        Gizmos.DrawSphere(pointB.transform.position, 1f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);

        //Gizmos.DrawSphere(this.gameObject.transform.position ,detectDistance);
    }
}
