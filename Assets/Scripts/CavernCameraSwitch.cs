using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernCameraSwitch : MonoBehaviour
{
   
    public Cinemachine.CinemachineVirtualCamera cavernCamera;

    private bool cavernCameraActive = false;
    public float canvernCameraShift = 5;

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            cavernCameraActive = true;
            StartCoroutine(SwitchToCavern());
            print("cavern cam");
        }
    }

    

    private IEnumerator SwitchToCavern()
    {
        if (cavernCameraActive == true)
            cavernCamera.Priority = 100;


        yield return new WaitForSeconds(canvernCameraShift);
        cavernCamera.Priority = 0;
        cavernCameraActive = false;
        this.enabled = false;
    }
}
