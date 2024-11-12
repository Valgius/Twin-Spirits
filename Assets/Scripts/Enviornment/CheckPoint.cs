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
    bool touchingCheckpoint;
    public GameObject button;

    CheckpointManager manager;
    public Vector2 respawnPoint;

    private Tutorial tutorial;

    void Start()
    {
        playerLeafRespawn = GameObject.Find("PlayerLeaf").GetComponent<PlayerRespawn>();
        playerSeaRespawn = GameObject.Find("PlayerSea").GetComponent<PlayerRespawn>();
        manager = FindObjectOfType<CheckpointManager>();
        tutorial = FindObjectOfType<Tutorial>();
        respawnPoint = transform.position;
    }

    private void Update()
    {

        if (touchingCheckpoint && Input.GetButtonDown("Interact"))
        {
            manager.CheckpointSelect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorial.CheckpointTeleportTutorial(true);
            touchingCheckpoint = true;
        }

        if (collision.gameObject.name == "PlayerLeaf")
        {
            playerLeafRespawn.respawnPoint = transform.position;
            print("hit checkpoint leaf");
        }
        if(collision.gameObject.name == "PlayerSea")
        {
            playerSeaRespawn.respawnPoint = transform.position;
            print("hit checkpoint sea");
        }

        if (collision.gameObject.CompareTag("Player") && !usedCheckPoint)
            UpdateFlag();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingCheckpoint = false;
            tutorial.CheckpointTeleportTutorial(false);
        }
    }

    private void UpdateFlag()
    {
        inActive.SetActive(false);
        active.SetActive(true);
        usedCheckPoint = true;
        _AM.PlaySFX("Checkpoint Active");
        manager.CheckpointActivate();
        tutorial.CheckpointTutorial();
    }
}
