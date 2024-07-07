using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    private PlayerRespawn playerRespawn;

    void Start()
    {
        playerRespawn = GameObject.Find("PlayerSea").GetComponent<PlayerRespawn>();
        playerRespawn = GameObject.Find("PlayerLeaf").GetComponent<PlayerRespawn>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 1;
            if (health <= 0)
                playerRespawn.Respawn();
        }
    }
}
