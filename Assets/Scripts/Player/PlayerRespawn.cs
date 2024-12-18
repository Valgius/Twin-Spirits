using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : GameBehaviour
{
    public Vector3 respawnPoint;
    private PlayerHealth playerHealth;
    private PlayerController playerController;
    FadeOut fadeOut;

    void Start()
    {
        playerHealth = this.gameObject.GetComponent<PlayerHealth>();
        fadeOut = FindObjectOfType<FadeOut>();
        playerController = GetComponent<PlayerController>();
    }

    public void Respawn()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        transform.position = respawnPoint;
        playerController.breathTimer = playerController.maxBreathTimer;
        playerHealth.MaxHealth();
        //_AM.PlaySFX("Revive");
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
