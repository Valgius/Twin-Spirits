using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : GameBehaviour
{
    [SerializeField] private GameObject fadeOutObject;
    public bool playerDie;

    public Animator fadeOutAni;
    public PlayerController playerControllerLeaf;
    public PlayerController playerControllerSea;
    private bool isLeaf;
    PlayerRespawn playerRespawn;
    public GameObject deathPanel;

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

    public void DisplayDeathScreen()
    {
        deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
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
        deathPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
