using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private PlayerRespawn playerRespawn;
    public GameObject active;
    public GameObject inActive;

    public bool usedCheckPoint;

    void Start()
    {
        playerRespawn = GameObject.Find("PlayerSea").GetComponent<PlayerRespawn>();
        playerRespawn = GameObject.Find("PlayerLeaf").GetComponent<PlayerRespawn>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (usedCheckPoint == false && (collision.gameObject.name == "PlayerSea" || collision.gameObject.name == "PlayerLeaf"))
        {
            playerRespawn.respawnPoint = transform.position;
            inActive.SetActive(false);
            active.SetActive(true);
            usedCheckPoint = true;
        }
    }
}
