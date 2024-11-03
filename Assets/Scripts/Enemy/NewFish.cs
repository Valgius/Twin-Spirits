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

    [Header("AI")]
    public float startSpeed = 20f;
    public float movementSpeed;
    public float chaseSpeed;
    public float attackDistance;
    private float detectCountdown = 5f;
    public float detectTime = 5f;
    public float detectDistance;
    public float attackTimer = 0;
    public bool hasAttacked;

    public GameObject fishAttackBox;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPoint = pointB.transform;
        detectCountdown = detectTime;
        newFishAttack = GetComponent<NewFishAttack>();
        attackTimer = 0;
        hasAttacked = false;
    }

    void Update()
    {
        float disToPlayer = Vector2.Distance(transform.position, playerSea.transform.position);

        if (disToPlayer < detectDistance && myPatrol == PatrolType.Patrol)
        {
            myPatrol = PatrolType.Detect;
        }
        else if(disToPlayer > detectDistance)
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
        
        if (currentPoint.position.x > transform.position.x && myPatrol != PatrolType.Detect)
        {
            transform.right = currentPoint.position - transform.position;
        }
        else if(currentPoint.position.x < transform.position.x && myPatrol != PatrolType.Detect)
        {
            transform.right = transform.position - currentPoint.position;
            
        }
        if(myPatrol != PatrolType.Detect)
            FlipSprite();
        else
            spriteRenderer.flipX = false;
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
        if(detectTime <= 0)
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
        currentPoint = playerSea.transform;
        Move();
        movementSpeed = chaseSpeed;

        if(Vector2.Distance(transform.position, playerSea.transform.position) > detectDistance)
        {
            currentPoint = pointA.transform;
            myPatrol = PatrolType.Patrol;
        }

        if(Vector2.Distance(transform.position, playerSea.position) < attackDistance)
        {
            myPatrol = PatrolType.Attack;
        }
    }

    void Attack()
    {
        if (!hasAttacked && attackTimer <= 0)
        {
            attackTimer = 2f;

        }

        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            fishAttackBox.SetActive(true);
        }
        else
        {
            fishAttackBox.SetActive(false);
        }
        
        if(Vector2.Distance(transform.position, playerSea.position) > attackDistance && !hasAttacked)
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
