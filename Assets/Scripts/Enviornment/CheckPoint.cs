using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : GameBehaviour
{
    private PlayerRespawn playerLeafRespawn;
    private PlayerRespawn playerSeaRespawn;
    public GameObject active;
    public GameObject inActive;

    public bool usedCheckPoint;
    public GameObject button;

    CheckpointManager manager;
    public Vector2 respawnPoint;


    void Start()
    {
        playerLeafRespawn = GameObject.Find("PlayerLeaf").GetComponent<PlayerRespawn>();
        playerSeaRespawn = GameObject.Find("PlayerSea").GetComponent<PlayerRespawn>();
        manager = FindObjectOfType<CheckpointManager>();
        respawnPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLeaf")
        {
            playerLeafRespawn.respawnPoint = transform.position;
        }
        if(collision.gameObject.name == "PlayerSea")
        {
            playerSeaRespawn.respawnPoint = transform.position;
        }

        if (!usedCheckPoint)
            UpdateFlag();
    }

    private void UpdateFlag()
    {
        inActive.SetActive(false);
        active.SetActive(true);
        usedCheckPoint = true;
        _AM.PlaySFX("Checkpoint Active");
        manager.CheckpointActivate();
    }
}
