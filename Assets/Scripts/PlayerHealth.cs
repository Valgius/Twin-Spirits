using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : GameBehaviour
{
    public int health;
    public int maxHealth;
    private PlayerRespawn playerRespawn;

    public Image healthFill;

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
            UpdateHealthBar(health, maxHealth);
            if (health <= 0)
            {
                playerRespawn.Respawn();
                health = maxHealth;
                UpdateHealthBar(health, maxHealth);
            }
        }
    }

    public void UpdateHealthBar(int _health, int _maxHealth)
    {
        healthFill.fillAmount = MapTo01(_health, 0, _maxHealth);
    }
}
