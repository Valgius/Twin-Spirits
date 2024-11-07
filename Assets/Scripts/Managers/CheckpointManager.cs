using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckpointManager : GameBehaviour
{
    //I am so so sorry for how shitty this code is.

    public List<GameObject> checkPointsLeaf;
    public List<GameObject> checkPointsSea;
    public List<GameObject> activeCheckPointsLeaf;
    public List<GameObject> activeCheckPointsSea;
    public List<GameObject> checkPointButtons = new List<GameObject>();

    public GameObject button;

    [SerializeField] private GameObject leafMenu;
    [SerializeField] private GameObject seaMenu;

    public bool checkpointLeaf;

    public Vector2 checkpointPosition;

    PlayerController playerControllerLeaf;
    PlayerController playerControllerSea;

    void Start()
    {
        playerControllerLeaf = GameObject.Find("PlayerLeaf").GetComponent<PlayerController>();
        playerControllerSea = GameObject.Find("PlayerSea").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //GetCheckPoints();
            //GetUI();
            CheckpointSelect();
        }
    }

    public void AddButton(bool isLeaf)
    {
        if (isLeaf)
        {
            Instantiate(button, leafMenu.transform);
            checkpointPosition = playerControllerLeaf.transform.position;
            checkpointLeaf = true;
        }
        else
        {
            Instantiate(button, seaMenu.transform);
            checkpointPosition = playerControllerSea.transform.position;
            checkpointLeaf = false;
        }
        
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
        foreach (Transform buttons in leafMenu.GetComponentInChildren<Transform>())
        {
            checkPointButtons.Add(buttons.gameObject);
            //CheckPointUI buttonUI = buttons.GetComponent<CheckPointUI>();
            //TMP_Text buttonText = buttons.GetComponentInChildren<TMP_Text>();
            //buttonText.text = "Checkpoint " + buttonUI.count;
        }
    }

    void CheckpointSelect()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        if (leafMenu.activeSelf || seaMenu.activeSelf)
        {
            leafMenu.SetActive(false);
            seaMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
            
        if (playerControllerLeaf.isActiveAndEnabled)
        {
            leafMenu.SetActive(true);
        }
        else if(playerControllerSea.isActiveAndEnabled)
        {
            seaMenu.SetActive(true);
        }
            
    }
}
