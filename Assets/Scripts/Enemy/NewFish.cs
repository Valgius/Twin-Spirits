using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewFish : GameBehaviour
{
    public PatrolType myPatrol;
    public BoxCollider2D fishCollider;
    LayerMask mask;

    [Header("Patrol Points")]
    public GameObject pointA;
    public GameObject pointB;

    public CircleCollider2D patrolZoneCollider;

    [Header("Transforms")]
    public Transform targetPoint;
    private Transform playerSea;

    SpriteRenderer spriteRenderer;
    PlayerHealth playerHealth;
    NewEnemyChaseZone newEnemyChaseZone;

    public GameObject patrolArea;

    [Header("Speed")]
    [SerializeField] private float startSpeed = 20f;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float chaseSpeed;

    [Header("Distance")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float detectDistance;
    [SerializeField] private float activationDistance = 50;
    [SerializeField] private float raycastDistance;

    [Header("Timers")]
    [SerializeField] private float detectTime = 5f;
    [SerializeField] private float detectCountdown = 1f;
    [SerializeField] private float attackTimer = 0;


    void Start()
    {
        //A whole lotta declares
        mask = LayerMask.GetMask("Ground");
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fishCollider = GetComponent<BoxCollider2D>();
        playerHealth = playerSea.GetComponent<PlayerHealth>();
        newEnemyChaseZone = patrolArea.GetComponent<NewEnemyChaseZone>();
        patrolZoneCollider = patrolArea.GetComponent<CircleCollider2D>();
        targetPoint = pointB.transform;
        detectCountdown = detectTime;
        attackTimer = 0;
    }

    void Update()
    {
        //Distance declaration
        float disToPlayer = Vector2.Distance(transform.position, playerSea.transform.position);
        CullEnemy(disToPlayer);
        if (disToPlayer > activationDistance)
            return;

        GetDistance(disToPlayer);

        //Switch cases for patrol types
        switch (myPatrol)
        {
            case PatrolType.Patrol:
                Patrol();
                break;
            case PatrolType.Detect:
                Detect();
                break;
            case PatrolType.Chase:
                Chase();
                break;
            case PatrolType.Attack:
                Attack();
                break;
        }

        //Look at currentPoint at most times.
        if (targetPoint.position.x > transform.position.x && myPatrol != PatrolType.Detect && myPatrol != PatrolType.Attack)
        {
            transform.right = targetPoint.position - transform.position;
        }
        else if (targetPoint.position.x < transform.position.x && myPatrol != PatrolType.Detect && myPatrol != PatrolType.Attack)
        {
            transform.right = transform.position - targetPoint.position;

        }
        //Flip sprites
        FlipSprite();

        //When within a certain distance, attack.
        if (Vector2.Distance(transform.position, playerSea.position) <= attackDistance)
        {
            myPatrol = PatrolType.Attack;
        }

        //Countdown timer for attack.
        if (attackTimer > 0)
        {
            movementSpeed = 0f;
            attackTimer -= Time.deltaTime;
        }

        ReturnToPatrol();

    }

    private void FixedUpdate()
    {
        Raycast();
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        NewTarget();
    //    }
    //}

    void Raycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector2.left, out hit, raycastDistance, mask))
        {
            NewTarget();
        }
    }

    void GetDistance(float disToPlayer)
    {
        if (disToPlayer < detectDistance && myPatrol == PatrolType.Patrol && newEnemyChaseZone.canFollow)
        {
            myPatrol = PatrolType.Detect;
        }
        else if (disToPlayer > detectDistance)
            transform.right = targetPoint.position - transform.position;
    }

    void ReturnToPatrol()
    {
        //When player dies return to patrol.
        if (playerHealth.health <= 0)
        {
            myPatrol = PatrolType.Patrol;
            targetPoint = pointA.transform;
        }
    }

    void Move()
    {
        //Move to currentPoint using movement speed.
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, movementSpeed * Time.deltaTime);
    }

    void NewTarget()
    {
        //Distance declaration for patrol function.
        float disToWaypoint = Vector2.Distance(transform.position, targetPoint.position);
        if (disToWaypoint < 1.5f)
        {
            //float radius = patrolZoneCollider.radius;
            //Vector2 center = patrolZoneCollider.transform.position;
            targetPoint.position = GetRandomPointInCircle();
            print(targetPoint.localPosition);

            //targetPoint = (targetPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }
    }

    Vector2 GetRandomPointInCircle()
    {
        // Get the radius of the circle
        float radius = patrolZoneCollider.radius;
        // Get the center position of the circle in world space
        Vector2 center = patrolZoneCollider.transform.position;

        // Generate a random angle between 0 and 2 * PI radians
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Generate a random distance from the center (square root distribution keeps points uniformly distributed)
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

        // Convert polar coordinates (angle and distance) to Cartesian coordinates
        float x = center.x + distance * Mathf.Cos(angle);
        float y = center.y + distance * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

    public void Patrol()
    {
        //Set speed to default speed and detect time.
        movementSpeed = startSpeed;
        detectTime = detectCountdown;
        Move();

        NewTarget();

    }

    void Detect()
    {
        //Look at player when detecting
        transform.right = playerSea.transform.position - transform.position;

        if(playerSea.transform.position.x < transform.position.x)
            spriteRenderer.flipY = true;
        else
            spriteRenderer.flipY = false;

        if (spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }

        //Stay still for a few seconds before chasing.
        movementSpeed = 0f;
        detectTime -= Time.deltaTime;
        if (detectTime <= 0)
        {
            myPatrol = PatrolType.Chase;

        }

        //If the player leaves detectDistance, return to patrol.
        if (Vector2.Distance(transform.position, playerSea.transform.position) > detectDistance && detectCountdown > 0)
        {
            myPatrol = PatrolType.Patrol;

        }

    }

    void Chase()
    {
        //Chase the player using chaseSpeed as movement speed
        targetPoint = playerSea.transform;
        Move();
        movementSpeed = chaseSpeed;

        //Return to patrol if the player leaves detect distance or patrol zone.
        if (Vector2.Distance(transform.position, playerSea.transform.position) > detectDistance || newEnemyChaseZone.canFollow == false)
        {
            targetPoint = pointA.transform;
            myPatrol = PatrolType.Patrol;
        }
    }

    void Attack()
    {
        //Attack player if attack timer is 0 and player health is greater than 0
        if(attackTimer <= 0 && playerHealth.health > 0)
        {
            print("hit");
            _AM.PlaySFX("Fish Attack");
            attackTimer = 1f;
            playerHealth.EnemyHit();
        }
        //Return to chase when leaving attackDistance.
        if(Vector2.Distance(transform.position, playerSea.position) > attackDistance)
        {
            myPatrol = PatrolType.Chase;
        }
    }


    void FlipSprite()
    {
        if(myPatrol != PatrolType.Detect)
        {
            //Flip sprite on x axis.
            if (targetPoint.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        
    }

    public void CullEnemy(float disToPlayer)
    {
        if (disToPlayer > activationDistance)
        {
            fishCollider.enabled = false;
            spriteRenderer.enabled = false;
        }
        else
        {
            spriteRenderer.enabled = true;
            fishCollider.enabled = true;
        }

        if(myPatrol != PatrolType.Detect)
        {
            spriteRenderer.flipY = false;
        }
            
    }
}
