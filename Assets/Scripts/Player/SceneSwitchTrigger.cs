using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    public PlayerSwitch playerSwitch;

    // Start is called before the first frame update
    void Start()
    {
        playerSwitch = GameObject.Find("GameManager").GetComponent<PlayerSwitch>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerSwitch.SwitchCharacter();
    }


}
