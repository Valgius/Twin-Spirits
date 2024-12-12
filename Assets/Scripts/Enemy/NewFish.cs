using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewFish : GameBehaviour
{
    public PatrolType myPatrol;
    public BoxCollider2D fishCollider;
    public Rigidbody2D rb;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    public GameObject startPoint;

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

    [Header("Timers")]
    public float detectTime = 2f;
    [SerializeField] private float detectCountdown = 1f;
    [SerializeField] private float attackTimer = 0;

    [Header("Animation")]
    public Animator mainAnim;
    public Animator[] smallAnim;
    public float minDelay = 0.1f;
    public float maxDelay = 0.5f;


    void Start()
    {
        //A whole lotta declares
        playerSea = GameObject.Find("PlayerSea").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        fishCollider = GetComponent<BoxCollider2D>();
        playerHealth = playerSea.GetComponent<PlayerHealth>();
        newEnemyChaseZone = patrolArea.GetComponent<NewEnemyChaseZone>();
        patrolZoneCollider = patrolArea.GetComponent<CircleCollider2D>();
        mainAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        detectCountdown = detectTime;
        attackTimer = 0;

        PlayMoveAnimationForChildren();
    }

    void Update()
    {
        //Distance declaration
        float disToPlayer = Vector2.Distance(transform.position, playerSea.transform.position);
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

        LookAtDestination();

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

        StartCoroutine(ReturnToPatrol());
    }

    void LookAtDestination()
    {
        //Look at currentPoint at most times.
        if (myPatrol != PatrolType.Detect && myPatrol != PatrolType.Attack)
        {
            Vector2 lookDirection = (targetPoint.position - transform.position);
            transform.right = lookDirection.normalized;
        }
    }

    void GetDistance(float disToPlayer)
    {
        if (disToPlayer < detectDistance && myPatrol == PatrolType.Patrol && newEnemyChaseZone.canFollow)
        {
            myPatrol = PatrolType.Detect;
        }
        else
            return;
    }

    IEnumerator ReturnToPatrol()
    {
        //When player dies return to patrol.
        if (playerHealth.health <= 0)
        {
            mainAnim.SetTrigger("isAttacking");
            yield return new WaitForSeconds(1);
            myPatrol = PatrolType.Patrol;
            //targetPoint = startPoint.transform;
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
            int rndPoint = Random.Range(0, patrolPoints.Length);
            targetPoint = patrolPoints[rndPoint];
        }
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
            targetPoint = startPoint.transform;
            myPatrol = PatrolType.Detect;
        }
    }

    void Attack()
    {
        //Attack player if attack timer is 0 and player health is greater than 0
        if (attackTimer <= 0 && playerHealth.health > 0)
        {
            print("hit");
            mainAnim.SetTrigger("isAttacking");
            _AM.PlaySFX("Fish Attack");
            playerHealth.EnemyHit();
            PlayAttackAnimationForChildren();
            attackTimer = 1.5f;
            
        }

        //Return to chase when leaving attackDistance.
        if(Vector2.Distance(transform.position, playerSea.position) > attackDistance)
        {
            myPatrol = PatrolType.Chase;
            PlayMoveAnimationForChildren();
        }
    }

    public void ToggleComponents(bool isActive)
    {
        if (isActive)
        {
            UnFreezeConstraints();
            spriteRenderer.enabled = true;
            fishCollider.enabled = true;
        }
        else
        {
            FreezeConstraints();
            spriteRenderer.enabled = false;
            fishCollider.enabled = false;
        }
    }

    private void FreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void UnFreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FlipSprite()
    {
        if(myPatrol != PatrolType.Detect)
        {
            //Flip sprite on x axis.
            if (targetPoint.position.x < transform.position.x)
            {
                spriteRenderer.flipY = true;
            }
            else
            {
                spriteRenderer.flipY = false;
            }
        }
    }

    /// <summary>
    /// Plays movement animation for the small fish.
    /// </summary>
    void PlayMoveAnimationForChildren()
    {
        foreach (Animator animator in smallAnim)
        {
            if (animator != null)
            {
                float delay = Random.Range(minDelay, maxDelay);
                StartCoroutine(PlayAnimationWithDelay(animator, "FishMove", delay));
            }
        }
    }

    /// <summary>
    /// Play attack animation for the small fish.
    /// </summary>
    void PlayAttackAnimationForChildren()
    {
        foreach (Animator animator in smallAnim)
        {
            if (animator != null)
            {
                float delay = Random.Range(minDelay, maxDelay);
                StartCoroutine(PlayAnimationWithDelay(animator, "FishAttack", delay));
            }
        }
    }

    /// <summary>
    /// Plays an animation after a short delay.
    /// </summary>
    /// <param name="animator">The animator component to animate.</param>
    /// <param name="animationName">Name of the animation to play.</param>
    /// <param name="delay">Delay before playing the animation.</param>
    /// <returns></returns>
    private IEnumerator PlayAnimationWithDelay(Animator animator, string animationName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play(animationName);
    }

}
