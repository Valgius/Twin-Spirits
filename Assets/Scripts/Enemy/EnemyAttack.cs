using System.Collections;
using UnityEngine;

public class EnemyAttack : GameBehaviour
{
    public EnemyPatrol enemyPatrol;

    public GameObject projectilePrefab;
    public float spiderFireRate = 1f;
    public Transform spiderFirePoint;

    public GameObject frogGas;
    public Transform frogFirePoint;

    public Transform playerTransform;

    public float attackTimer;

    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<BoxCollider2D>(), projectilePrefab.GetComponent<BoxCollider2D>());
        playerTransform = enemyPatrol.closestPlayer;
        //GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        if(attackTimer > 0) //While attack timer is more then 0, fish will freeze in place for chomp animation to play when implemented.
        {
            attackTimer -= Time.deltaTime;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            rb.constraints = ~RigidbodyConstraints2D.FreezePosition;
        }
            
    }

    public IEnumerator FishAttack()
    {
        //When fish attacks, start timer for fish to freeze. Once done, the fish will return to chase.
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Fish Attack");
        attackTimer = 3f;
        //PlayAnimation("Attack");
        enemyPatrol.myPatrol = PatrolType.Chase;
        yield return null;
    }

    public IEnumerator FrogAttack()
    {
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Frog Attack");
        enemyPatrol.ChangeSpeed(0);
        yield return new WaitForSeconds(0.5f);
        GasAttack();
        //PlayAnimation("Attack");
        yield return new WaitForSeconds(3);
        enemyPatrol.ChangeSpeed(enemyPatrol.mySpeed);
        enemyPatrol.myPatrol = PatrolType.Chase;
    }

    public IEnumerator SpiderAttack()
    {
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Spider Attack");
        enemyPatrol.ChangeSpeed(0);
        Fire(playerTransform.position);
        //PlayAnimation("Attack");
        yield return new WaitForSeconds(spiderFireRate);
        enemyPatrol.myPatrol = PatrolType.Detect;
    }

    void Fire(Vector2 targetPosition)
    {
        if (projectilePrefab && spiderFirePoint)
        {
            // Instantiate the projectile
            GameObject projectile = Instantiate(projectilePrefab, spiderFirePoint.position, Quaternion.identity);
            // Initialize the projectile with the target position
            projectile.GetComponent<Projectile>()?.Initialize(targetPosition);
        }
    }

    void GasAttack()
    {
        Instantiate(frogGas, frogFirePoint.position, Quaternion.identity);
    }
}
