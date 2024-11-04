using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAttackBox : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
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
