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

    public Cinemachine.CinemachineVirtualCamera seaCamera;
    public Cinemachine.CinemachineVirtualCamera leafCamera;
    public Cinemachine.CinemachineVirtualCamera leafOrbCamera;
    public Cinemachine.CinemachineVirtualCamera seaOrbCamera;

    private PlayerController playerLeafController;
    private PlayerController playerSeaController;

    public PlayerSwitch playerSwitch;
    public int switchNumber;

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
        if (switchNumber == playerSwitch.switchCount)
        {
            switch (playerCharacter)
            {
                case PlayerCharacter.Leaf:
                    //When Player hits the trigger zone and has the corresponding orb, drop the orb to set location.
                    if (collision.gameObject.CompareTag("Player") && playerLeafController.hasSeaOrb)
                    {

                        print("Drop Orb Sea");
                        AnimateSeaOrb();
                        playerLeafController.ToggleHasSeaOrb();
                        //New orb is set as the sea orb in Orb Manager Script as old one would be disabled.
                        //_OM.orbSea = Instantiate(seaOrb, dropZone.transform.position, transform.rotation);
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
                        //_OM.orbLeaf = Instantiate(leafOrb, dropZone.transform.position, transform.rotation);
                        if (playerSeaController.hasSeaOrb != true)
                            _OM.orbPanelSea.SetActive(false);
                    }
                    break;
            }
            playerSwitch.switchCount++;
        }
        else
            return;
    }
    public void AnimateLeafOrb()
    {
        orb1.transform.position = this.transform.position;
        orb1.SetActive(true);
        orb1.GetComponent<CircleCollider2D>().enabled = false;

        leafOrbCamera.Priority = 15;

        animator.SetTrigger("LeafOrbTransition");
        print("leaf animation play");
    }

    public void AnimateSeaOrb()
    {
        orb2.transform.position = this.transform.position;
        orb2.SetActive(true);
        orb2.GetComponent<CircleCollider2D>().enabled = false;

        seaOrbCamera.Priority = 15;

        animator.SetTrigger("SeaOrbTransition");
        print("sea animation play");
    }

    public void AfterLeafAnimation()
    {
        orb1.GetComponent<CircleCollider2D>().enabled = true;
        print("afterleafAnimation");
        leafOrbCamera.Priority = 0;
        playerSwitch.isLeafActive = false;
        playerSwitch.SwitchCharacter();
    }

    public void AfterSeaAnimation()
    {
        orb2.GetComponent<CircleCollider2D>().enabled = true;
        print("afterseaAnimation");
        seaOrbCamera.Priority = 0;
        playerSwitch.isLeafActive = true;
        playerSwitch.SwitchCharacter();
    }
}
