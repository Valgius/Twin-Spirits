using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseZone : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private EnemyPatrol enemyPatrol;

    public void Start()
    {
        enemyPatrol = enemy.GetComponent<EnemyPatrol>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("zone Entered");
            enemyPatrol.currentPoint = enemyPatrol.closestPlayer;
            enemyPatrol.myPatrol = PatrolType.Chase;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            print("zone Exited");
            enemyPatrol.ChangeSpeed(0);
            Wait(1);

            // Switch patrol type and current point
            enemyPatrol.myPatrol = PatrolType.Patrol; // Reset to patrol mode
            enemyPatrol.currentPoint = (enemyPatrol.currentPoint == enemyPatrol.pointA.transform)
                ? enemyPatrol.pointB.transform
                : enemyPatrol.pointA.transform; // Switch to the other point
            enemyPatrol.ChangeSpeed(enemyPatrol.baseSpeed);
        }
    }

    private IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
