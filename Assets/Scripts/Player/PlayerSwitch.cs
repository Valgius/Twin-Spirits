using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitch : GameBehaviour
{
    public GameObject playerLeaf;
    public GameObject playerLeafUI;
    public GameObject playerLeafCamera;

    public GameObject playerSea;
    public GameObject playerSeaUI;
    public GameObject playerSeaCamera;

    public Rigidbody2D seaRb;
    public Rigidbody2D leafRb;

    public Cinemachine.CinemachineVirtualCamera seaCamera;

    public GameObject musicTriggerObj;
    private MusicTrigger[] musicTriggers;

    FadeOut fadeOut;

    public bool isLeafActive;

    public int switchCount = 0;

    

   

    void Start()
    {
        isLeafActive = true;
        StartCoroutine(ActivateLeaf());
        Physics2D.IgnoreCollision(playerSea.GetComponent<BoxCollider2D>(), playerLeaf.GetComponent<BoxCollider2D>());
        fadeOut = FindObjectOfType<FadeOut>();

        musicTriggers = musicTriggerObj.GetComponentsInChildren<MusicTrigger>();
        
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
        fadeOut.playerSwitch = true;

        if (isLeafActive == true)
            StartCoroutine(ActivateSea());
        else
            StartCoroutine(ActivateLeaf());
    }

    private IEnumerator ActivateLeaf()
    {
       
        playerLeaf.GetComponent<PlayerController>().enabled = true;
        playerLeaf.GetComponent<PlayerRespawn>().enabled = true;
        playerLeaf.GetComponent<BoxCollider2D>().enabled = true;
        playerLeafUI.SetActive(true);
        playerLeafCamera.SetActive(true);

        playerSea.GetComponent<PlayerController>().enabled = false;
        playerSea.GetComponent<PlayerRespawn>().enabled = false;
        playerSea.GetComponent<BoxCollider2D>().enabled = false;
        playerSeaUI.SetActive(false);
        playerSeaCamera.SetActive(false);
        
        seaCamera.Priority = 5;

        isLeafActive = true;

        seaRb.constraints = RigidbodyConstraints2D.FreezeAll;
        leafRb.constraints = RigidbodyConstraints2D.None;
        leafRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(2);
        leafRb.AddForce(Vector2.down, ForceMode2D.Impulse);
        _EM.player = playerLeaf.transform;

        CheckMusic();
    }

    private IEnumerator ActivateSea()
    {
        
        playerLeaf.GetComponent<PlayerController>().enabled = false;
        playerLeaf.GetComponent<PlayerRespawn>().enabled = false;
        playerLeaf.GetComponent<BoxCollider2D>().enabled = false;
        playerLeafUI.SetActive(false);
        playerLeafCamera.SetActive(false);

        playerSea.GetComponent<PlayerController>().enabled = true;
        playerSea.GetComponent<PlayerRespawn>().enabled = true;
        playerSea.GetComponent<BoxCollider2D>().enabled = true;
        playerSeaUI.SetActive(true);
        playerSeaCamera.SetActive(true);
    
        seaCamera.Priority = 11;

        isLeafActive = false;

        leafRb.constraints = RigidbodyConstraints2D.FreezeAll;
        seaRb.constraints = RigidbodyConstraints2D.None;
        seaRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(2);
        seaRb.AddForce(Vector2.down, ForceMode2D.Impulse);
        _EM.player = playerSea.transform;

        CheckMusic();
    }

    public void CheckMusic()
    {
        foreach(MusicTrigger trigger in musicTriggers)
        {
            if (isLeafActive == true)
                trigger.ChangeMusicLeaf();
            if (isLeafActive == false)
                trigger.ChangeMusicSea();
        }

        fadeOut.playerSwitch = false;
    }
}
