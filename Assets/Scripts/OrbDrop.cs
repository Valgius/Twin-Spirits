using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDrop : GameBehaviour
{
    public GameObject playerLeaf;
    public GameObject playerSea;
    public Transform dropZone;
    public GameObject leafOrb;
    public GameObject SeaOrb;

    private PlayerController playerLeafController;
    private PlayerController playerSeaController;
    
    void Start()
    {
        dropZone = this.transform;
        playerLeafController = playerLeaf.GetComponent<PlayerController>();
        playerSeaController = playerSea.GetComponent<PlayerController>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerLeafController.hasSeaOrb)
        {
            playerLeafController.ToggleHasSeaOrb();
            Instantiate(SeaOrb.gameObject, this.transform);
        }
    }
}
