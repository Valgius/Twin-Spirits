using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbAnimations : MonoBehaviour
{
    public Animator Orbanimatons;

    public GameObject seaOrbCam;
    public GameObject leafOrbCam;

    // Start is called before the first frame update
    void Start()
    {
        seaOrbCam.SetActive(false);
        leafOrbCam.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
