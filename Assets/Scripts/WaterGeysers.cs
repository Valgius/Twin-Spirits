using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGeysers : GameBehaviour
{
    [SerializeField] private GameObject seaPlayer;
    private PlayerController playerController;
    [SerializeField] private float forceSpeed;
    


    void Start()
    {
        seaPlayer = FindObjectOfType<PlayerController>().gameObject;
        playerController = seaPlayer.GetComponent<PlayerController>();
        
    }

    private void FixedUpdate()
    {
        if (playerController.touchingGeyser)
        {
            
            playerController.GetComponent<Rigidbody2D>().AddRelativeForce(transform.right * forceSpeed, ForceMode2D.Impulse);
            //playerController.GetComponent<Rigidbody2D>().velocity = this.gameObject.transform.right * forceSpeed;
            //playerController.isSwimming = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.touchingGeyser = true;
            print(this.gameObject.name);
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
