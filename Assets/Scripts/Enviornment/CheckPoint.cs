using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : GameBehaviour
{
    private PlayerRespawn playerLeafRespawn;
    private PlayerRespawn playerSeaRespawn;
    public GameObject active;
    public GameObject inActive;
    [SerializeField] private GameObject respawnPos;

    public bool usedCheckPoint;
    bool touchingCheckpoint;
    public GameObject button;
    public bool isSceneSwitch;

    CheckpointManager manager;
    public Vector2 respawnPoint;

    private Tutorial tutorial;

    void Start()
    {
        playerLeafRespawn = GameObject.Find("PlayerLeaf").GetComponent<PlayerRespawn>();
        playerSeaRespawn = GameObject.Find("PlayerSea").GetComponent<PlayerRespawn>();
        manager = FindObjectOfType<CheckpointManager>();
        tutorial = FindObjectOfType<Tutorial>();
        if(respawnPos != null)
        {
            respawnPoint = respawnPos.transform.position;
        }
        
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
        if (collision.gameObject.CompareTag("Player") && !isSceneSwitch)
        {
            tutorial.CheckpointTeleportTutorial(true);
            touchingCheckpoint = true;
        }

        if (collision.gameObject.name == "PlayerLeaf")
        {
            playerLeafRespawn.respawnPoint = respawnPoint;
            print("hit checkpoint leaf");
        }
        if(collision.gameObject.name == "PlayerSea")
        {
            playerSeaRespawn.respawnPoint = respawnPoint;
            print("hit checkpoint sea");
        }

        if (collision.gameObject.CompareTag("Player") && !usedCheckPoint && !isSceneSwitch)
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
