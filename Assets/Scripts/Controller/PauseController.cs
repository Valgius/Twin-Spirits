using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : GameBehaviour
{
    public GameObject pausePanel;
    private bool paused;

    private void Start()
    {
        paused = false;
        pausePanel.SetActive(paused);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        paused = !paused;
        pausePanel.SetActive(paused);
        Time.timeScale = paused ? 0 : 1; 
    }
}
