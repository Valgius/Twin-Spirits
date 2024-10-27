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
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
             gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckSwitchNumber();
        //StartCoroutine(SwitchDelay());
    }

    private IEnumerator SwitchDelay()
    {
        yield return new WaitForSeconds(sceneDelay);
        playerSwitch.SwitchCharacter();
        playerSwitch.switchCount++;
        hasSwitched = true;
    }

    private void CheckSwitchNumber()
    {
        if (switchNumber == playerSwitch.switchCount)
            StartCoroutine(SwitchDelay());
    }
}
