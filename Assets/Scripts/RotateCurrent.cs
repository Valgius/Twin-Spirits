using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCurrent : MonoBehaviour
{
    public GameObject rotatingCurrent;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            rotatingCurrent.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        rotatingCurrent.transform.eulerAngles = new Vector3(0, 0, 0);
        print("current trigger entered");

        if (other.CompareTag("backtrackStopper"))
        {
            rotatingCurrent.transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    
}
