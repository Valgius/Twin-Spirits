using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointManager : GameBehaviour
{
    public List<GameObject> checkPointsLeaf;
    public List<GameObject> checkPointsSea;
    public List<GameObject> activeCheckPointsLeaf;
    public List<GameObject> activeCheckPointsSea;
    public List<GameObject> checkPointButtons = new List<GameObject>();

    public GameObject button;

    [SerializeField] private GameObject menu;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GetCheckPoints();
            GetUI();
        }
    }

    public void AddButton()
    {
        Instantiate(button, menu.transform);
    }

    void GetCheckPoints()
    {
        foreach (GameObject checkPoint in activeCheckPointsLeaf)
        {
            if (checkPoint == null)
                return;


            print(checkPoint.name);
        }

        foreach (GameObject checkPoint in activeCheckPointsSea)
        {
            if (checkPoint == null)
                return;

            print(checkPoint.name);
        }
    }

    void GetUI()
    {
        foreach (Transform buttons in menu.GetComponentInChildren<Transform>())
        {
            checkPointButtons.Add(buttons.gameObject);
            CheckPointUI buttonUI = buttons.GetComponent<CheckPointUI>();
            TMP_Text buttonText = buttons.GetComponentInChildren<TMP_Text>();
            buttonText.text = "Checkpoint " + buttonUI.count;
        }
    }

}
