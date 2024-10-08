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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //playerController.GetComponent<Rigidbody2D>().AddForce(new Vector2(forceSpeed, 0), ForceMode2D.Force);
            playerController.GetComponent<Rigidbody2D>().AddForce(transform.right * forceSpeed, ForceMode2D.Force);
            playerController.maxSwimSpeed = forceSpeed;
        }
        else
            playerController.maxSwimSpeed = playerController.swimSpeed;
    }
}
