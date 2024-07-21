using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrog : GameBehaviour
{
    public PatrolType myPatrol;
    public float baseSpeed = 1f;
    public float mySpeed = 1f;
    public float jumpForce = 1f;

    public GameObject pointA;
    public GameObject pointB;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D enemyCollider;
    //private Animator anim;
    private Transform currentPoint;

    [Header("AI")]
    public float attackDistance = 2;
    public float detectTime = 5f;
    public float detectDistance = 10f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        mySpeed = baseSpeed;
        enemyCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPatrol == PatrolType.Die)
            return; //cancels anything after this line

        //Switching patrol states logic
        switch (myPatrol)
        {
            case PatrolType.Patrol:
                Vector2 point = currentPoint.position - transform.position;
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


                if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
                {
                    currentPoint = pointA.transform;
                    FlipSprite();
                }

                if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
                {
                    currentPoint = pointB.transform;
                    FlipSprite();
                }

                break;
            case PatrolType.Detect:

                break;
            case PatrolType.Chase:
                break;
        }

    }

    private void FlipSprite()
    {
        Vector3 transformScale = transform.localScale;
        transformScale.x *= -1;
        transform.localScale = transformScale;
    }

    void ChangeSpeed(float _speed)
    {
        mySpeed = _speed;
    }

    IEnumerator Attack()
    {
        myPatrol = PatrolType.Attack;
        ChangeSpeed(0);
        //PlayAnimation("Attack");
        yield return new WaitForSeconds(1);
        ChangeSpeed(mySpeed);
        myPatrol = PatrolType.Chase;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
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
}
