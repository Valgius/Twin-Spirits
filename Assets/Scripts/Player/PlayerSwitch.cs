using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{
    public GameObject playerLeaf;
    public GameObject playerLeafUI;
    public GameObject playerLeafCamera;

    public GameObject playerSea;
    public GameObject playerSeaUI;
    public GameObject playerSeaCamera;

    public Rigidbody2D seaRb;
    public Rigidbody2D leafRb;

    public bool isLeafActive;
    void Start()
    {
        isLeafActive = true;
        ActivateLeaf();
        Physics2D.IgnoreCollision(playerSea.GetComponent<BoxCollider2D>(), playerLeaf.GetComponent<BoxCollider2D>());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SwitchCharacter();
        }


    }

    public void SwitchCharacter()
    {
        if (isLeafActive == true)
            ActivateSea();
        else
            ActivateLeaf();
    }

    private void ActivateLeaf()
    {
        playerLeaf.GetComponent<PlayerController>().enabled = true;
        playerLeafUI.SetActive(true);
        playerLeafCamera.SetActive(true);

        playerSea.GetComponent<PlayerController>().enabled = false;
        playerSeaUI.SetActive(false);
        playerSeaCamera.SetActive(false);

        isLeafActive = true;

        seaRb.constraints = RigidbodyConstraints2D.FreezeAll;
        leafRb.constraints = RigidbodyConstraints2D.None;
        leafRb.constraints = RigidbodyConstraints2D.FreezeRotation;


    }

    private void ActivateSea()
    {
        playerLeaf.GetComponent<PlayerController>().enabled = false;
        playerLeafUI.SetActive(false);
        playerLeafCamera.SetActive(false);

        playerSea.GetComponent<PlayerController>().enabled = true;
        playerSeaUI.SetActive(true);
        playerSeaCamera.SetActive(true);

        isLeafActive = false;

        leafRb.constraints = RigidbodyConstraints2D.FreezeAll;
        seaRb.constraints = RigidbodyConstraints2D.None;
        seaRb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }
}
