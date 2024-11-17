using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseController : GameBehaviour
{
    public GameObject pausePanel;
    public bool paused;

    //public bool eventPause;
    //public GameObject pauseButton;
    ControllerMenuManager controlManager;

    [SerializeField] private GameObject firstPauseButton;

    private void Start()
    {
        paused = false;
        pausePanel.SetActive(paused);
        Time.timeScale = 1;
        //eventPause = EventSystem.current;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controlManager = FindObjectOfType<ControllerMenuManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            Pause();

        //if (paused && Input.GetAxis("Vertical") != 0 && !eventPause)
        //{
        //    EventSystem.current.firstSelectedGameObject = pauseButton;
        //    //EventSystem.current.SetSelectedGameObject(pauseButton);
        //}
        
    }

    public void Pause()
    {
        if (!paused)
        {
            controlManager.SetActiveButton(firstPauseButton);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            ToggleAudio();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ToggleAudio();
        }
            

        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0 : 1; 

    }

    void ToggleAudio()
    {
        _AM.ToggleMusic();
        _AM.ToggleAmbience();
        _AM.ToggleSFX();
    }
}
