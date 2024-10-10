using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttackBox : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public EnemyAttack enemyAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth.EnemyHit();
            enemyAttack.FishAttack();
        }
    }
}
