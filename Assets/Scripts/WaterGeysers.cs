using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterGeysers : GameBehaviour
{
    [SerializeField] private GameObject seaPlayer;
    private PlayerController playerController;
    [SerializeField] private float forceSpeed;
    public Vector2 forwardDirection;
    

    void Start()
    {
        seaPlayer = FindObjectOfType<PlayerController>().gameObject;
        playerController = seaPlayer.GetComponent<PlayerController>();
        
    }


    private void FixedUpdate()
    {
        if (playerController.touchingGeyser)
        {
            
            playerController.GetComponent<Rigidbody2D>().AddRelativeForce(transform.right * forceSpeed);
            //playerController.GetComponent<Rigidbody2D>().velocity = forwardDirection * forceSpeed;
            //playerController.isSwimming = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(this.gameObject.name + this.transform.rotation);
        forwardDirection = this.gameObject.GetComponent<Transform>().transform.right;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.touchingGeyser = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //playerController.maxSwimSpeed = playerController.swimSpeed;
            //playerController.isSwimming = true;
            playerController.touchingGeyser = false;
        }
    }
}
