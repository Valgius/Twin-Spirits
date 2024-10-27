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

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private FadeOut fadeOut;
    [SerializeField] private Animator damage;
    public float hitCooldown = 0;
    
    [Header("Cinemachine Shake")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    public float screenShake = 0;

    void Start()
    {
        fadeOut = FindObjectOfType<FadeOut>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    /// <summary>
    /// Determines player health, runs timer cooldowns.
    /// </summary>
    private void Update()
    {
        //If health is over max health, set health to max health.
        if (health > numOfHearts)
            MaxHealth();

        //Display hearts based on how much health player has.
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
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //When player touches an enemy that is not a fish, damage player and set screenshake timer.
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

    /// <summary>
    /// Player takes damage, sets a hitCooldown, and runs a damage animation. If the player has no health, disable sprite and sets playerDie for fadeOut script to run.
    /// </summary>
    public void EnemyHit()
    {
        if(health > 0 && hitCooldown <= 0)
        {
            _AM.PlaySFX("Player Hit");
            health -= 1;
            hitCooldown = 1;
            screenShake = 0.5f;
            damage.SetTrigger("Damage");
        }
        
        if (health <= 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            fadeOut.playerDie = true;
            DisableAnimations();
            _AM.PlaySFX("Player Die");
            _AM.PlaySFX("Death"); 
        }
    }

    public void MaxHealth()
    {
        health = numOfHearts;
    }

    /// <summary>
    /// Sets screen shake amount.
    /// </summary>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    public void Noise(float amplitude, float frequency)
    {
        noise.m_AmplitudeGain = amplitude;
        noise.m_FrequencyGain = frequency;
    }

    /// <summary>
    /// Turns off all animations.
    /// </summary>
    void DisableAnimations()
    {
        damage.SetFloat("Speed", 0);
        damage.SetBool("isClimbing", false);
        damage.SetBool("isJumping", false);
        damage.SetBool("isDashing", false);
        damage.SetBool("isSwimming", false);
    }
}
