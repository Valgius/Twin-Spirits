using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : GameBehaviour
{
    public Vector3 respawnPoint;
    private PlayerHealth playerHealth;
    FadeOut fadeOut;

    void Start()
    {
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
        fadeOut = FindObjectOfType<FadeOut>();
    }

    public void Respawn()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        fadeOut.playerDie = false;
        transform.position = respawnPoint;
        playerHealth.MaxHealth();
        _AM.PlaySFX("Revive");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            fadeOut.playerDie = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
