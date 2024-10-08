using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Get the EnemyPatrol component from the exiting object
            EnemyPatrol enemyPatrol = other.GetComponent<EnemyPatrol>();
            if (enemyPatrol != null)
            {
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
    }

    public IEnumerator Wait(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
