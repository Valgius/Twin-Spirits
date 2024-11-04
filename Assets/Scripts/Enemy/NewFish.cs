using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFish : GameBehaviour
{
    public PatrolType myPatrol;
    NewFishAttack newFishAttack;

    [Header("Patrol Points")]
    public GameObject pointA;
    public GameObject pointB;

    [Header("Transforms")]
    private Rigidbody2D rb;
    public Transform currentPoint;
    private Transform playerSea;

    SpriteRenderer spriteRenderer;
    PlayerHealth playerHealth;
    NewEnemyChase newEnemyChase;

    public GameObject patrolArea;

    [Header("AI")]
    public float startSpeed = 20f;
    public float movementSpeed;
    public float chaseSpeed;
    public float attackDistance;
    public float detectCountdown = 2f;
    public float detectTime = 3f;
    public float detectDistance;
    public float attackTimer = 0;
    public bool isAttacking;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPoint = pointB.transform;
        detectCountdown = detectTime;
        newFishAttack = GetComponent<NewFishAttack>();
        playerHealth = playerSea.GetComponent<PlayerHealth>();
        newEnemyChase = patrolArea.GetComponent<NewEnemyChase>();
        attackTimer = 0;
        isAttacking = false;
    }

    void Update()
    {
        float disToPlayer = Vector2.Distance(transform.position, playerSea.transform.position);

        if (disToPlayer < detectDistance && myPatrol == PatrolType.Patrol && newEnemyChase.canFollow)
        {
            myPatrol = PatrolType.Detect;
        }
        else if (disToPlayer > detectDistance)
            transform.right = currentPoint.position - transform.position;

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

        if (currentPoint.position.x > transform.position.x && myPatrol != PatrolType.Detect && myPatrol != PatrolType.Attack)
        {
            transform.right = currentPoint.position - transform.position;
        }
        else if (currentPoint.position.x < transform.position.x && myPatrol != PatrolType.Detect && myPatrol != PatrolType.Attack)
        {
            transform.right = transform.position - currentPoint.position;

        }
        if (myPatrol != PatrolType.Detect)
            FlipSprite();
        else
            spriteRenderer.flipX = false;

        if (Vector2.Distance(transform.position, playerSea.position) <= attackDistance)
        {
            myPatrol = PatrolType.Attack;
        }

        if (attackTimer > 0)
        {
            movementSpeed = 0f;
            attackTimer -= Time.deltaTime;
        }

        if (playerHealth.health <= 0)
        {
            myPatrol = PatrolType.Patrol;
            currentPoint = pointA.transform;
        }

    }

    public void Patrol()
    {
        movementSpeed = startSpeed;
        detectTime = 2f;
        Move();

        float disToWaypoint = Vector2.Distance(transform.position, currentPoint.position);

        if (disToWaypoint < 1.5f)
        {
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }

    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, currentPoint.position, movementSpeed * Time.deltaTime);
    }

    void Detect()
    {
        transform.right = playerSea.position - transform.position;

        movementSpeed = 0f;
        detectTime -= Time.deltaTime;
        if (detectTime <= 0)
        {
            myPatrol = PatrolType.Chase;

        }
        if (Vector2.Distance(transform.position, playerSea.transform.position) > detectDistance && detectCountdown > 0)
        {
            myPatrol = PatrolType.Patrol;

        }

    }

    void Chase()
    {
        //if (newEnemyChase.canFollow == false)
        //{
        //    myPatrol = PatrolType.Patrol;
        //    currentPoint = pointA.transform;
        //    return;
        //}


        currentPoint = playerSea.transform;
        Move();
        movementSpeed = chaseSpeed;

        if (Vector2.Distance(transform.position, playerSea.transform.position) > detectDistance || newEnemyChase.canFollow == false)
        {
            currentPoint = pointA.transform;
            myPatrol = PatrolType.Patrol;
        }
    }

    void Attack()
    {
        if(attackTimer <= 0 && playerHealth.health > 0)
        {
            print("hit");
            _AM.PlaySFX("Fish Attack");
            attackTimer = 1f;
            playerHealth.EnemyHit();
        }
        

        if(Vector2.Distance(transform.position, playerSea.position) > attackDistance && !isAttacking)
        {
            myPatrol = PatrolType.Chase;
        }
    }


    void FlipSprite()
    {
        if(currentPoint.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
}
