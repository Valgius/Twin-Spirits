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

    public PlayerSwitch playerSwitch;

    public Animator animator;

    public GameObject orb1;
    public GameObject orb2;





    [SerializeField] enum PlayerCharacter { Leaf, Sea }

    [SerializeField] PlayerCharacter playerCharacter;


    void Start()
    {
        //Set Player controllers to be caaalled on.
        playerLeafController = playerLeaf.GetComponent<PlayerController>();
        playerSeaController = playerSea.GetComponent<PlayerController>();

        playerSwitch = GameObject.Find("GameManager").GetComponent<PlayerSwitch>();
    }

    private void Update()
    {
        if (playerLeafController.isActiveAndEnabled)
        {
            playerCharacter = PlayerCharacter.Leaf;
        }
        else
            playerCharacter = PlayerCharacter.Sea;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (playerCharacter)
        {
            case PlayerCharacter.Leaf:
                //When Player hits the trigger zone and has the corresponding orb, drop the orb to set location.
                if (collision.gameObject.CompareTag("Player") && playerLeafController.hasSeaOrb)
                {

                    print("Drop Orb Sea");
                    playerLeafController.ToggleHasSeaOrb();
                    //New orb is set as the sea orb in Orb Manager Script as old one would be disabled.
                    _OM.orbSea = Instantiate(seaOrb, dropZone.transform.position, transform.rotation);
                    if (playerLeafController.hasLeafOrb != true)
                        _OM.orbPanelLeaf.SetActive(false);
                }
                break;
            case PlayerCharacter.Sea:
                if (collision.gameObject.CompareTag("Player") && playerSeaController.hasLeafOrb)
                {
                    print("Drop Orb Leaf");
                    AnimateLeafOrb();
                    playerSeaController.ToggleHasLeafOrb();
                    //Same as previous line but for leaf orb.
                    _OM.orbLeaf = Instantiate(leafOrb, dropZone.transform.position, transform.rotation);
                    if (playerSeaController.hasSeaOrb != true)
                        _OM.orbPanelSea.SetActive(false);
                }
                break;
        }
    }
    public void AnimateLeafOrb()
    {

        orb1.SetActive(true);
        animator.SetTrigger("LeafOrbTransition");

    }

    public void AnimateSeaOrb()
    {
        orb2.SetActive(true);
        animator.SetTrigger("SeaOrbTransition");
    }

    
}
