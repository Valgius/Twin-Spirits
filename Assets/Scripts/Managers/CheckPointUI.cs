using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointUI : GameBehaviour
{
    public int count;
    CheckpointManager manager;
    TMP_Text text;

    PlayerRespawn respawn;
    public Vector2 respawnPosition;

    PlayerController playerControllerLeaf;
    PlayerController playerControllerSea;

    CheckPoint checkPoint;

    void Start()
    {
        playerControllerLeaf = GameObject.Find("PlayerLeaf").GetComponent<PlayerController>();
        playerControllerSea = GameObject.Find("PlayerSea").GetComponent<PlayerController>();
        manager = FindObjectOfType<CheckpointManager>();
        text = GetComponentInChildren<TMP_Text>();

        
        if (manager.checkpointLeaf)
        {
            LeafCheckpoint();
        }
        else
            SeaCheckpoint();
           
    }


    void Update()
    {
        
    }

    public void LeafCheckpoint()
    {
        count = manager.activeCheckPointsLeaf.Count;
        respawnPosition = playerControllerLeaf.transform.position;
        text.text = "Checkpoint " + count;
    }

    void SeaCheckpoint()
    {
        count = manager.activeCheckPointsSea.Count;
        respawnPosition = playerControllerSea.transform.position;
        text.text = "Checkpoint " + count;
    }

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
        respawn.gameObject.transform.position = respawnPosition;
    }
}
