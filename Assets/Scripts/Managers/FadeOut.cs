using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : GameBehaviour
{
    [SerializeField] private GameObject fadeOutObject;
    public bool playerDie;

    [SerializeField] private Animator fadeOutAni;
    [SerializeField] private PlayerController playerControllerLeaf;
    [SerializeField] private PlayerController playerControllerSea;
    private bool isLeaf;
    private PlayerRespawn playerRespawn;

    void Start()
    {
        fadeOutAni = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (playerDie)
        {
            fadeOutAni.SetBool("Die", true);
            fadeOutAni.SetBool("Respawn", false);
        }
        else
        {
            fadeOutAni.SetBool("Die", false);
            fadeOutAni.SetBool("Respawn", true);
        }
    }


    public void GetCurrentPlayer()
    {
        if(playerControllerLeaf.isActiveAndEnabled)
        {
            playerRespawn = playerControllerLeaf.GetComponent<PlayerRespawn>();
        }
        else if(playerControllerSea.isActiveAndEnabled)
        {
            playerRespawn = playerControllerSea.GetComponent<PlayerRespawn>();
        }
    }

    public void RespawnPlayer()
    {
        GetCurrentPlayer();
        playerRespawn.Respawn();
        
    }
}
