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
    [SerializeField] private bool leafCheckPoint;

    public Vector2 respawnPoint;

    CheckpointManager checkpointManager;

    void Start()
    {
        playerLeafRespawn = GameObject.Find("PlayerLeaf").GetComponent<PlayerRespawn>();
        playerSeaRespawn = GameObject.Find("PlayerSea").GetComponent<PlayerRespawn>();
        checkpointManager = FindObjectOfType<CheckpointManager>();
        respawnPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (usedCheckPoint == false && (collision.gameObject.name == "PlayerLeaf"))
        {
            respawnPoint = playerLeafRespawn.respawnPoint = transform.position;
            UpdateFlag();
        }
        if(usedCheckPoint == false && (collision.gameObject.name == "PlayerSea"))
        {
            respawnPoint = playerSeaRespawn.respawnPoint = transform.position;
            UpdateFlag();
        }
    }

    private void UpdateFlag()
    {
        inActive.SetActive(false);
        active.SetActive(true);
        usedCheckPoint = true;
        if (leafCheckPoint)
        {
            checkpointManager.activeCheckPointsLeaf.Add(gameObject);
            checkpointManager.checkPointsLeaf.Remove(gameObject);
            checkpointManager.AddButton(isLeaf: true);
        }
        else
        {
            checkpointManager.activeCheckPointsSea.Add(gameObject);
            checkpointManager.checkPointsSea.Remove(gameObject);
            checkpointManager.AddButton(isLeaf: false);
        }
            
        
        _AM.PlaySFX("Checkpoint Active");
    }
}
