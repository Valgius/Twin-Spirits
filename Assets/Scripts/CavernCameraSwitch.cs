using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernCameraSwitch : GameBehaviour
{
   
    public Cinemachine.CinemachineVirtualCamera cavernCamera;

    public bool cavernCameraActive = false;
    public float canvernCameraShift = 5;
    [SerializeField] private bool hasActivated = false;


    PlayerController playerController;
    private GameObject playerSea;

    private void Start()
    {
        playerSea = GameObject.Find("PlayerSea");
        playerController = playerSea.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hasActivated)
        {
            playerController.canMove = false;
            //_AsM.ToggleScriptUse(false);
            cavernCameraActive = true;
            StartCoroutine(SwitchToCavern());
            print("cavern cam");
            hasActivated = true;
        }
    }

    private IEnumerator SwitchToCavern()
    {
        if (cavernCameraActive == true)
            cavernCamera.Priority = 100;

        yield return new WaitForSeconds(canvernCameraShift);
        cavernCamera.Priority = 0;
        playerController.canMove = true;
        cavernCameraActive = false;
        this.enabled = false;
        yield return new WaitForSeconds(3);
        //_AsM.ToggleScriptUse(true);
    }
}
