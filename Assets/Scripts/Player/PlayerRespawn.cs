using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
    }

    public void Respawn()
    {
        transform.position = respawnPoint;
        playerHealth.MaxHealth();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            Respawn();
        }
    }
}
