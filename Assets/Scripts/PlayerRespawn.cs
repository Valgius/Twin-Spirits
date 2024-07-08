using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GameObject.Find("PlayerSea").GetComponent<PlayerHealth>();
        playerHealth = GameObject.Find("PlayerLeaf").GetComponent<PlayerHealth>();
    }

    public void Respawn()
    {
        transform.position = respawnPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            Respawn();
            playerHealth.MaxHealth();
        }
    }
}
