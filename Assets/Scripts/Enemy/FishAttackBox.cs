using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttackBox : MonoBehaviour
{
    public EnemyAttack enemyAttack;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.EnemyHit();
            }
            else
            {
                Debug.LogWarning("PlayerHealth not Found");
            }
        }
    }
}
