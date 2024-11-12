using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : GameBehaviour
{
    [SerializeField] private GameObject loadScene;
    [SerializeField] private GameObject playerLeaf;
    [SerializeField] private GameObject playerSea;
    private bool leafFinished;
    private bool seaFinished;
    private PlayerController playerController;
    private Rigidbody2D playerRb;

    private void Start()
    {
        leafFinished = false;
        seaFinished = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerLeaf)
        {
            leafFinished = true;
            CheckEndStatus();
        }

        if (collision.gameObject == playerSea)
        {
            seaFinished = true;
            CheckEndStatus();
            seaStatic();
        }
    }

    private void CheckEndStatus()
    {
        if (leafFinished && seaFinished)
            loadScene.SetActive(true);
    }

    private void seaStatic()
    {
        playerController = playerLeaf.GetComponent<PlayerController>();
        playerRb = playerLeaf.GetComponent<Rigidbody2D>();
    }

}
