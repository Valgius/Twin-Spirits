using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointUI : GameBehaviour
{
    PlayerRespawn respawn;
    public Vector2 respawnPosition;
    [SerializeField] private GameObject checkPoint;

    PlayerController playerControllerLeaf;
    PlayerController playerControllerSea;
    CheckpointManager manager;

    void Awake()
    {
        playerControllerLeaf = GameObject.Find("PlayerLeaf").GetComponent<PlayerController>();
        playerControllerSea = GameObject.Find("PlayerSea").GetComponent<PlayerController>();
        respawnPosition = checkPoint.transform.position;
        manager = FindObjectOfType<CheckpointManager>();
    }

    /// <summary>
    /// When button is pressed, teleport player to connceted checkpoint.
    /// </summary>
    public void Teleport()
    {
        if (playerControllerLeaf.isActiveAndEnabled)
        {
            respawn = playerControllerLeaf.GetComponent<PlayerRespawn>();
            
        }
        else if (playerControllerSea.isActiveAndEnabled)
        {
            respawn = playerControllerSea.GetComponent<PlayerRespawn>();
        }
        manager.leafMenu.SetActive(false);
        manager.seaMenu.SetActive(false);
        manager.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        respawn.gameObject.transform.position = respawnPosition;
        Time.timeScale = 1;
    }
}
