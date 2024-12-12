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

    PlayerController playerControllerSea;
    PlayerController playerControllerLeaf;

    public Cinemachine.CinemachineVirtualCamera seaCamera;

    public GameObject musicTriggerObj;
    private MusicTrigger[] musicTriggers;

    FadeOut fadeOut;

    public bool isLeafActive;

    public int switchCount = 1;

    void Start()
    {
        playerControllerSea = playerSea.GetComponent<PlayerController>();
        playerControllerLeaf = playerLeaf.GetComponent<PlayerController>();
        isLeafActive = true;
        StartCoroutine(ActivateLeaf());
        Physics2D.IgnoreCollision(playerSea.GetComponent<BoxCollider2D>(), playerLeaf.GetComponent<BoxCollider2D>());
        fadeOut = FindObjectOfType<FadeOut>();

        musicTriggers = musicTriggerObj.GetComponentsInChildren<MusicTrigger>();
    }
    
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    SwitchCharacter();
        //    switchCount--;
        //}
    }

    public void SwitchCharacter()
    {

        if (isLeafActive == true)
            StartCoroutine(ActivateSea());
        else
            StartCoroutine(ActivateLeaf());

        switchCount++;
    }

    private IEnumerator ActivateLeaf()
    {
       
        playerLeaf.GetComponent<PlayerController>().enabled = true;
        playerLeaf.GetComponent<PlayerRespawn>().enabled = true;
        playerLeaf.GetComponent<BoxCollider2D>().enabled = true;
        playerLeaf.GetComponent<CapsuleCollider2D>().enabled = true;
        playerLeafUI.SetActive(true);
        playerLeafCamera.SetActive(true);

        playerSea.GetComponent<PlayerController>().enabled = false;
        playerSea.GetComponent<PlayerRespawn>().enabled = false;
        playerSea.GetComponent<BoxCollider2D>().enabled = false;
        playerSeaUI.SetActive(false);
        playerSeaCamera.SetActive(false);
        playerControllerSea.breathTimer = playerControllerSea.maxBreathTimer;
        
        seaCamera.Priority = 5;

        isLeafActive = true;

        seaRb.constraints = RigidbodyConstraints2D.FreezeAll;
        leafRb.constraints = RigidbodyConstraints2D.None;
        leafRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _EM.player = playerLeaf.transform;
        //_AsM.player = playerLeaf.transform;
        yield return new WaitForSeconds(2);
        leafRb.AddForce(Vector2.down, ForceMode2D.Impulse);
        CheckMusic();
    }

    private IEnumerator ActivateSea()
    {
        
        playerLeaf.GetComponent<PlayerController>().enabled = false;
        playerLeaf.GetComponent<PlayerRespawn>().enabled = false;
        playerLeaf.GetComponent<BoxCollider2D>().enabled = false;
        playerLeaf.GetComponent<CapsuleCollider2D>().enabled = false;
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
        _EM.player = playerSea.transform;
        //_AsM.player = playerSea.transform;
        yield return new WaitForSeconds(2);
        seaRb.AddForce(Vector2.down, ForceMode2D.Impulse);
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
    }
}
