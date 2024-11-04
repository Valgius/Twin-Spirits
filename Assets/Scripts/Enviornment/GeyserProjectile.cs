using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserProjectile : GameBehaviour
{
    [SerializeField] private float timer = 1.2f;

    [SerializeField] private Quaternion projectileDirection;
    WaterGeyser geyser;
    PlayerHealth playerHealth;
    PlayerController playerController;

    public GameObject playerSea;
    
    void Start()
    {
        playerSea = GameObject.Find("PlayerSea");
        playerController = playerSea.GetComponent<PlayerController>();
        geyser = transform.parent.GetComponent<WaterGeyser>();
        //this.transform.rotation = this.GetComponentInParent<Transform>().rotation;
        this.transform.rotation = transform.parent.rotation;
        projectileDirection = gameObject.transform.rotation;
    }

    
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
        transform.rotation = projectileDirection;
    }

    private void FixedUpdate()
    {
        //Apply speed depending on parent geyser direction.
        switch (geyser.direction)
        {
            case WaterGeyser.Direction.Left:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(-geyser.projectileForce, 0);
                break;
                case WaterGeyser.Direction.Right:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(geyser.projectileForce, 0);
                break;
                case WaterGeyser.Direction.Up:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, geyser.projectileForce);
                break;
                case WaterGeyser.Direction.Down:
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -geyser.projectileForce);
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController.isDashing != true)
        {
            if(collision.GetComponent<PlayerHealth>() != null)
            {
                playerHealth = collision.GetComponent<PlayerHealth>();
                playerHealth.EnemyHit();
            }
            
        }
    }
}
