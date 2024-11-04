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

    [Header("Transforms")]
    public Transform currentPoint;
    private Transform playerSea;

    SpriteRenderer spriteRenderer;
    PlayerHealth playerHealth;
    NewEnemyChase newEnemyChase;

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
        currentPoint = pointB.transform;
        detectCountdown = detectTime;
        playerHealth = playerSea.GetComponent<PlayerHealth>();
        newEnemyChase = patrolArea.GetComponent<NewEnemyChase>();
        attackTimer = 0;
    }

    void Update()
    {
        //Distance declaration for update
        float disToPlayer = Vector2.Distance(transform.position, playerSea.transform.position);
        if (disToPlayer < detectDistance && myPatrol == PatrolType.Patrol && newEnemyChase.canFollow)
        {
            myPatrol = PatrolType.Detect;
        }
        else if (disToPlayer > detectDistance)
            transform.right = currentPoint.position - transform.position;

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
        if (currentPoint.position.x > transform.position.x && myPatrol != PatrolType.Detect && myPatrol != PatrolType.Attack)
        {
            transform.right = currentPoint.position - transform.position;
        }
        else if (currentPoint.position.x < transform.position.x && myPatrol != PatrolType.Detect && myPatrol != PatrolType.Attack)
        {
            transform.right = transform.position - currentPoint.position;

        }
        //Flip sprites
        if (myPatrol != PatrolType.Detect)
            FlipSprite();
        else
            spriteRenderer.flipX = false;

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

        //When player dies return to patrol.
        if (playerHealth.health <= 0)
        {
            myPatrol = PatrolType.Patrol;
            currentPoint = pointA.transform;
        }

    }

    public void Patrol()
    {
        //Set speed to default speed and detect time.
        movementSpeed = startSpeed;
        detectTime = detectCountdown;
        Move();

        //Distance declaration for patrol function.
        float disToWaypoint = Vector2.Distance(transform.position, currentPoint.position);
        if (disToWaypoint < 1.5f)
        {
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }

    }

    void Move()
    {
        //Move to currentPoint using movement speed.
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, movementSpeed * Time.deltaTime);
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
        currentPoint = playerSea.transform;
        Move();
        movementSpeed = chaseSpeed;

        //Return to patrol if the player leaves detect distance or patrol zone.
        if (Vector2.Distance(transform.position, playerSea.transform.position) > detectDistance || newEnemyChase.canFollow == false)
        {
            currentPoint = pointA.transform;
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
        //Flip sprite on x axis.
        if(currentPoint.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
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
