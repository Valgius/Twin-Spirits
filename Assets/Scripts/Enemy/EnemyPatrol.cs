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

    public float baseSpeed = 1f;
    public float mySpeed = 1f;
    public float jumpForce = 1f;

    public GameObject pointA;
    public GameObject pointB;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D enemyCollider;
    //private Animator anim;
    public Transform currentPoint;
    public Transform nextPoint;
    private Transform playerSea;
    private Transform playerLeaf;

    [Header("AI")]
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

        //Always get the distance between the player and this object
        float distToSea = Vector3.Distance(transform.position, playerSea.transform.position);
        float distToLeaf = Vector3.Distance(transform.position, playerLeaf.transform.position);

        if (distToLeaf <= detectDistance && myPatrol != PatrolType.Attack)
        {
            if (myPatrol != PatrolType.Chase)
            {
                myPatrol = PatrolType.Detect;
            }
        }

        if (distToSea <= detectDistance && myPatrol != PatrolType.Attack)
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
                switch (myEnemy)
                {
                    case EnemyType.Fish:
                        // Calculate distance to current waypoint
                        float distanceToWaypoint = Vector2.Distance(transform.position, currentPoint.position);

                        // If close to current waypoint, switch to the next one
                        if (distanceToWaypoint < 1f)
                        {
                            currentPoint = nextPoint;
                        }

                        // Move towards current waypoint
                        Vector2 fishPoint = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
                        rb.MovePosition(fishPoint);

                        break;

                    case EnemyType.Spider:
                        Vector2 spiderPoint = currentPoint.position - transform.position;
                        if (currentPoint == pointB.transform)
                            rb.velocity = new Vector2(0, mySpeed);
                        else
                            rb.velocity = new Vector2(0, -mySpeed);
                        break;
                    case EnemyType.Frog:
                        Vector2 frogPoint = currentPoint.position - transform.position;
                        if (currentPoint == pointB.transform)
                        {
                            rb.velocity = new Vector2(mySpeed, 0);
                            jump();
                        }

                        else
                        {
                            rb.velocity = new Vector2(-mySpeed, 0);
                            jump();
                        }
                        break;
                }
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
               //Stop our speed
                ChangeSpeed(0);

                //Decrement our detect time
                detectTime -= Time.deltaTime;
                if (distToSea <= detectDistance)
                {
                    switch (myEnemy)
                    {
                        case EnemyType.Fish:
                            currentPoint = playerSea;
                            myPatrol = PatrolType.Chase;
                            break;
                        case EnemyType.Frog:
                            myPatrol = PatrolType.Chase;
                            currentPoint = playerSea;
                            break;
                        case EnemyType.Spider:
                            myPatrol = PatrolType.Attack;
                            break;
                    }
                    detectTime = 5;
                }
                 
                if (distToLeaf <= detectDistance)
                {
                    switch (myEnemy)
                    {
                        case EnemyType.Fish:
                            myPatrol = PatrolType.Chase;
                            currentPoint = playerLeaf;
                            break;
                        case EnemyType.Frog:
                            myPatrol = PatrolType.Chase;
                            currentPoint = playerLeaf;
                            break;
                        case EnemyType.Spider:
                            StartCoroutine(enemyAttack.SpiderAttack());
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

                Vector2 targetPosition = Vector2.MoveTowards(transform.position, currentPoint.position, mySpeed * Time.deltaTime);
                rb.MovePosition(targetPosition);

                //increase the speed of which to chase the player
                ChangeSpeed(mySpeed * 2);

                //If the player gets outside the detect distance, go back to the detect state.
                if (distToLeaf > detectDistance)
                    myPatrol = PatrolType.Detect;
                if (distToSea > detectDistance)
                    myPatrol = PatrolType.Detect;

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

    private void jump()
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
