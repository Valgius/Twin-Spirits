using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFish : GameBehaviour
{
    public PatrolType myPatrol;
    public EnemyAttack enemyAttack;

    [Header("Patrol Points")]
    public GameObject pointA;
    public GameObject pointB;

    [Header("Transforms")]
    private Rigidbody2D rb;
    public Transform currentPoint;
    private Transform playerSea;
    private Transform playerLeaf;
    public Transform closestPlayer;

    SpriteRenderer spriteRenderer;

    [Header("AI")]
    public float startSpeed = 20f;
    public float movementSpeed;
    public float chaseSpeed;
    public float attackDistance;
    private float detectCooldown = 5f;
    public float detectTime = 5f;
    public float detectDistance;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        playerLeaf = GameObject.Find("PlayerLeaf").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentPoint = pointB.transform;
        movementSpeed = startSpeed;
        detectCooldown = detectTime;
    }

    void Update()
    {
        float disToPlayer = Vector2.Distance(transform.position, playerSea.transform.position);

        if(disToPlayer <= detectDistance && myPatrol != PatrolType.Attack || myPatrol != PatrolType.Chase)
        {
            myPatrol = PatrolType.Detect;
        }

        if(myPatrol != PatrolType.Attack)
        {
            Vector2.MoveTowards(transform.position, currentPoint.position, movementSpeed * Time.deltaTime);
        }
    }
}
