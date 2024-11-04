using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    public PlayerSwitch playerSwitch;
    public int switchNumber;
    public bool hasSwitched = false;
    public float sceneDelay;

    // Start is called before the first frame update
    void Start()
    {
        playerSwitch = GameObject.Find("GameManager").GetComponent<PlayerSwitch>();
        hasSwitched = false;
    }

    private void Update()
    {
        if (hasSwitched == true)
        {
            gameObject.SetActive(false);
           
        }
        else
        {
             gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckSwitchNumber();
    }

    private IEnumerator SwitchDelay()
    {
        print("sceneswitch triggered");
        yield return new WaitForSeconds(sceneDelay);
        playerSwitch.SwitchCharacter();
        hasSwitched = true;
    }

    private void CheckSwitchNumber()
    {
        if (switchNumber == playerSwitch.switchCount)
            StartCoroutine(SwitchDelay());
    }
}
