using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : GameBehaviour
{
    [SerializeField] private GameObject fadeOutObject;
    public bool playerDie;
    public bool playerSwitch;

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
        }
        else
        {
            fadeOutAni.SetBool("Die", false);
        }

        if (playerSwitch)
        {
            fadeOutAni.SetBool("Switch", true);
        }
        else
        {
            fadeOutAni.SetBool("Switch", false);
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

    public void ReactivatePlayer()
    {
        playerDie = false;
        if(playerControllerLeaf.isActiveAndEnabled)
        {
            playerControllerLeaf.GetComponent<PlayerController>().isGrounded = true;
        }
        else if (playerControllerSea.isActiveAndEnabled)
        {
            playerControllerSea.GetComponent<PlayerController>().isGrounded = true;
        }
    }
}
