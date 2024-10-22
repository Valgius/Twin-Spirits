using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseController : GameBehaviour
{
    public GameObject pausePanel;
    [SerializeField] private bool paused;

    public bool eventPause;
    public GameObject pauseButton;


    private void Start()
    {
        paused = false;
        pausePanel.SetActive(paused);
        Time.timeScale = 1;
        eventPause = EventSystem.current;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            Pause();

        if (paused && Input.GetAxis("Vertical") != 0 && !eventPause)
        {
            EventSystem.current.SetSelectedGameObject(pauseButton);
        }
        
    }

    public void Pause()
    {
        if (!paused)
        {
            EventSystem.current.SetSelectedGameObject(pauseButton);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
            

        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0 : 1; 

    }
}
