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

    CheckpointManager checkpointManager;

    void Start()
    {
        playerLeafRespawn = GameObject.Find("PlayerLeaf").GetComponent<PlayerRespawn>();
        playerSeaRespawn = GameObject.Find("PlayerSea").GetComponent<PlayerRespawn>();
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (usedCheckPoint == false && (collision.gameObject.name == "PlayerLeaf"))
        {
            playerLeafRespawn.respawnPoint = transform.position;
            UpdateFlag();
        }
        if(usedCheckPoint == false && (collision.gameObject.name == "PlayerSea"))
        {
            playerSeaRespawn.respawnPoint = transform.position;
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
        }
        else
        {
            checkpointManager.activeCheckPointsSea.Add(gameObject);
            checkpointManager.checkPointsSea.Remove(gameObject);
        }
            
        
        _AM.PlaySFX("Checkpoint Active");
    }
}
