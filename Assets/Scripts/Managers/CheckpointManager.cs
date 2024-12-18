using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CheckpointManager : GameBehaviour
{
    public List<GameObject> checkPointsLeaf;
    public List<GameObject> checkPointsSea;

    public GameObject leafMenu;
    public GameObject seaMenu;
    public bool isPaused = false;
    [SerializeField] private GameObject firstButtonLeaf;
    [SerializeField] private GameObject firstButtonSea;

    public Vector2 checkpointPosition;

    PlayerController playerControllerLeaf;
    PlayerController playerControllerSea;
    ControllerMenuManager controlManager;

    void Start()
    {
        playerControllerLeaf = GameObject.Find("PlayerLeaf").GetComponent<PlayerController>();
        playerControllerSea = GameObject.Find("PlayerSea").GetComponent<PlayerController>();
        controlManager = FindObjectOfType<ControllerMenuManager>();
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CheckpointSelect();
        }
        
    }
    
    /// <summary>
    /// Checks to see if each checkpoint has been used, if it has, then show button in checkpoint menu. If not, hide button.
    /// </summary>
    public void CheckpointActivate()
    {
        foreach (GameObject checkPoint in checkPointsLeaf)
        {
            CheckPoint _C = checkPoint.GetComponent<CheckPoint>();
            if (_C.usedCheckPoint == false)
            {
                _C.button.SetActive(false);
            }
            else
                _C.button.SetActive(true);
        }
        foreach (GameObject checkPoint in checkPointsSea)
        {
            CheckPoint _C = checkPoint.GetComponent<CheckPoint>();
            if(_C.usedCheckPoint == false)
            {
                _C.button.SetActive(false);
            }
            else
                _C.button.SetActive(true);
        }
    }

    /// <summary>
    /// Check which character is being used and display corresponding menu.
    /// </summary>
    public void CheckpointSelect()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
        isPaused = true;

        if (leafMenu.activeSelf || seaMenu.activeSelf)
        {
            leafMenu.SetActive(false);
            seaMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            isPaused = false;
            return;
        }
            
        if (playerControllerLeaf.isActiveAndEnabled)
        {
            leafMenu.SetActive(true);
            controlManager.SetActiveButton(firstButtonLeaf);
        }
        else if(playerControllerSea.isActiveAndEnabled)
        {
            controlManager.SetActiveButton(firstButtonSea);
            seaMenu.SetActive(true);
        }
            
    }

}
