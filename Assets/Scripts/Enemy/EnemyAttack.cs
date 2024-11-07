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

    public GameObject fishAttackBox;

    public Transform playerTransform;

    public float attackTimer;


    void Update()
    {
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<BoxCollider2D>(), projectilePrefab.GetComponent<BoxCollider2D>());
        playerTransform = enemyPatrol.closestPlayer;

        attackTimer -= Time.deltaTime;
    }

    public IEnumerator FishAttack()
    {
        if (attackTimer < 0)
        {
            enemyPatrol.myPatrol = PatrolType.Attack;
            print("Fish Attack");
            enemyPatrol.enemyAnim.SetBool("IsAttacking", true);
            fishAttackBox.SetActive(true);
            _AM.PlaySFX("Fish Attack");
            yield return new WaitForSeconds(0.5f);
            fishAttackBox.SetActive(false);
            enemyPatrol.enemyAnim.SetBool("IsAttacking", false);
            attackTimer = 1.5f;
        }

        enemyPatrol.myPatrol = PatrolType.Chase;
        yield return null;
    }

    public IEnumerator FrogAttack()
    {
        if(attackTimer < 0)
        {
            enemyPatrol.myPatrol = PatrolType.Attack;
            print("Frog Attack");
            enemyPatrol.ChangeSpeed(0);
            enemyPatrol.enemyAnim.SetBool("IsAttacking", true);
            enemyPatrol.enemyAnim.SetBool("isJumping", false);
            yield return new WaitForSeconds(0.5f);
            GasAttack();
            enemyPatrol.enemyAnim.SetBool("IsAttacking", false);
            attackTimer = 1.5f;
        }

        enemyPatrol.ChangeSpeed(enemyPatrol.mySpeed);
        enemyPatrol.myPatrol = PatrolType.Chase;
    }

    public IEnumerator SpiderAttack()
    {
        enemyPatrol.myPatrol = PatrolType.Attack;
        print("Spider Attack");
        enemyPatrol.ChangeSpeed(0);
        enemyPatrol.enemyAnim.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(0.1f);
        Fire(playerTransform.position);
        _AM.PlaySFX("Spider Attack");

        yield return new WaitForSeconds(spiderFireRate);
        enemyPatrol.enemyAnim.SetBool("IsAttacking", false);
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
        _AM.PlaySFX("Frog Attack");
    }
}
