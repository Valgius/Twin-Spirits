using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbDrop : GameBehaviour
{
    public GameObject playerLeaf;
    public GameObject playerSea;
    public GameObject dropZone;
    public GameObject leafOrb;
    public GameObject seaOrb;

    private PlayerController playerLeafController;
    private PlayerController playerSeaController;
    
    
    void Start()
    {
        //Set Player controllers to be called on.
        playerLeafController = playerLeaf.GetComponent<PlayerController>();
        playerSeaController = playerSea.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When Player hits the trigger zone and has the corresponding orb, drop the orb to set location.
        if (collision.gameObject.CompareTag("Player") && playerLeafController.hasSeaOrb)
        {
            print("Drop Orb Sea");
            playerLeafController.ToggleHasSeaOrb();
            //New orb is set as the sea orb in Orb Manager Script as old one would be disabled.
            _OM.orbSea = Instantiate(seaOrb, dropZone.transform.position, transform.rotation);
            _OM.orbPanelLeaf.SetActive(false);
            
        }
        else if (collision.gameObject.CompareTag("Player") && playerSeaController.hasLeafOrb)
        {
            print("Drop Orb Leaf");
            playerSeaController.ToggleHasLeafOrb();
            //Same as previous line but for leaf orb.
            _OM.orbLeaf = Instantiate(leafOrb, dropZone.transform.position, transform.rotation);
            _OM.orbPanelSea.SetActive(false);
            
        }
    }
}
