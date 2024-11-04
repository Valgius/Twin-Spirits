using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFish : GameBehaviour
{
    public PatrolType myPatrol;
    public BoxCollider2D fishCollider;

    [Header("Patrol Points")]
    public GameObject pointA;
    public GameObject pointB;

    public CircleCollider2D patrolZoneCollider;

    //Idle Point
    private Transform travelPoint;

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

    [Header("Timers")]
    [SerializeField] private float detectTime = 5f;
    [SerializeField] private float detectCountdown = 1f;
    [SerializeField] private float attackTimer = 0;


    void Start()
    {
        //A whole lotta declares
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
        //Distance declaration for update
        float disToPlayer = Vector2.Distance(transform.position, playerSea.transform.position);
        if (disToPlayer < detectDistance && myPatrol == PatrolType.Patrol && newEnemyChaseZone.canFollow)
        {
            myPatrol = PatrolType.Detect;
        }
        else if (disToPlayer > detectDistance)
            transform.right = targetPoint.position - transform.position;

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

    public void Patrol()
    {
        //Set speed to default speed and detect time.
        movementSpeed = startSpeed;
        detectTime = detectCountdown;
        Move();

        //Get the area of the chase zone
        //Get two random numbers, throw them into vector2

        //Distance declaration for patrol function.
        float disToWaypoint = Vector2.Distance(transform.position, targetPoint.position);
        if (disToWaypoint < 1.5f)
        {
            float radius = patrolZoneCollider.radius;
            Vector2 center = patrolZoneCollider.transform.position;
            targetPoint.position = new Vector3(center.x + Random.Range(-radius, radius), center.y + Random.Range(-radius, radius), transform.position.z);
            print(targetPoint.position);

            //targetPoint = (targetPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }

    }

    void Detect()
    {
        //Look at player when detecting
        transform.right = playerSea.position - transform.position;

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

    public void CullEnemy(bool isActive)
    {
        if (isActive)
        {
            this.gameObject.SetActive(true);
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            this.gameObject.SetActive(false);
        }
            
    }
}
