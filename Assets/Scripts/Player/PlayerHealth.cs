using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerHealth : GameBehaviour
{
    public int health;
    public int numOfHearts;
    private PlayerRespawn playerRespawn;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    FadeOut fadeOut;
    [SerializeField] private Animator damage;
    public float hitCooldown = 0;
    private float screenShake = 0;
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        playerRespawn = this.gameObject.GetComponent<PlayerRespawn>();
        fadeOut = FindObjectOfType<FadeOut>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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

        if (hitCooldown > 0)
        {
            hitCooldown -= Time.deltaTime;

        }

        if (screenShake > 0)
        {
            screenShake -= Time.deltaTime;
            Noise(1, 1);
        }
        else
            Noise(0, 0);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Noise(1, 1);
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<EnemyPatrol>().myEnemy != EnemyType.Fish && hitCooldown <= 0)
        {
            EnemyHit();
            screenShake = 1;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && hitCooldown <= 0)
        {
            EnemyHit();
        }
    }

    public void EnemyHit()
    {
        if(health > 0)
        {
            _AM.PlaySFX("Player Hit");
            health -= 1;
            hitCooldown = 1;
            damage.SetTrigger("Damage");
        }
        
        if (health <= 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            fadeOut.playerDie = true;
            _AM.PlaySFX("Player Die");
        }
    }

    public void MaxHealth()
    {
        health = numOfHearts;
    }

    void Noise(float amplitude, float frequency)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;
    }
}
