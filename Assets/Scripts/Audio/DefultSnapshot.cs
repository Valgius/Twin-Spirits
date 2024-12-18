using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefultSnapshot : GameBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _AM.TransitionToSnapshot("Start", 0);
    }
}
