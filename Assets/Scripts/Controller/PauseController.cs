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
        }
        else
            EventSystem.current.SetSelectedGameObject(null);

        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0 : 1; 
    }
}
