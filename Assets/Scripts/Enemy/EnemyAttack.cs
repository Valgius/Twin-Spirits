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

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<BoxCollider2D>(), projectilePrefab.GetComponent<BoxCollider2D>());
        playerTransform = enemyPatrol.closestPlayer;
    }

    public IEnumerator FishAttack()
    {
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Fish Attack");
        yield return new WaitForSeconds(1);
        attackTimer = 2f;
        while (attackTimer > 0)
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<BoxCollider2D>(), enemyPatrol.closestPlayer.GetComponent<BoxCollider2D>());
            attackTimer -= Time.deltaTime;
        }
        
        //PlayAnimation("Attack");
        enemyPatrol.ChangeSpeed(0);
        yield return new WaitForSeconds(3);
        enemyPatrol.ChangeSpeed(20);
        enemyPatrol.myPatrol = PatrolType.Chase;
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
