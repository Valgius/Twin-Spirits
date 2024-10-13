using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCurrent : MonoBehaviour
{
    public GameObject rotatingCurrent;
    //seperates the two triggers so one will stop the player from backtracking
    public bool isBackTrackStopper;

    public WaterFlow waterFlow;

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        rotatingCurrent.transform.eulerAngles = new Vector3(0, 0, 0);
    //    }
    //}

    private void Start()
    {
        waterFlow = GameObject.Find("RotatingWaterFlow").GetComponent<WaterFlow>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rotatingCurrent.transform.eulerAngles = new Vector3(0, 0, 0);
            print("current trigger entered");
            waterFlow.ResetCurrentDirection();

        }
       

        if (isBackTrackStopper == true)
        {
            rotatingCurrent.transform.eulerAngles = new Vector3(0, 0, 90);
            waterFlow.ResetCurrentDirection();
        }
    }

    
}
