using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseController : GameBehaviour
{
    public GameObject pausePanel;
    public GameObject audioPanel;
    public GameObject mainPanel;
    public bool paused;
    public bool audioMenu;

    ControllerMenuManager controlManager;

    [SerializeField] private GameObject firstPauseButton;

    private void Start()
    {
        paused = false;
        pausePanel.SetActive(paused);
        audioMenu = false;
        audioPanel.SetActive(audioMenu);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        controlManager = FindObjectOfType<ControllerMenuManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
            Pause();
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
            ResetPanel();
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

    public void ToggleAudioPanel()
    {
        audioMenu = !audioMenu;
        audioPanel.SetActive(audioMenu);
        mainPanel.SetActive(!audioMenu);
    }

    public void ResetPanel()
    {
        audioPanel.SetActive(false);
        mainPanel.SetActive(true);
        audioMenu = false;
    }
}
