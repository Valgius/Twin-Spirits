using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : GameBehaviour
{
    public int health;
    public int numOfHearts;
    private PlayerRespawn playerRespawn;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    void Start()
    {
        playerRespawn = this.gameObject.GetComponent<PlayerRespawn>();
        
    }

    private void Update()
    {
        if (health > numOfHearts)
            MaxHealth();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;

            if (i < numOfHearts)
                hearts[i].enabled = true;
            else
                hearts[i].enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<EnemyPatrol>().myEnemy != EnemyType.Fish)
        {
            EnemyHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyHit();
        }
    }

    public void EnemyHit()
    {
        _AM.PlaySFX("Player Hit");
        health -= 1;
        if (health <= 0)
        {
            _AM.PlaySFX("Player Die");
            playerRespawn.Respawn();
            MaxHealth();
        }
    }

    public void MaxHealth()
    {
        health = numOfHearts;
    }


}
