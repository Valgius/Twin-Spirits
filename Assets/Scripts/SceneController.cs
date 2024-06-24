using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : GameBehaviour
{
    public GameObject loadScene;
    public string scene;

    //Changes Scene to the one named in string
    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Loads out Title scene, Must be called Title exactly
    public string ToTitleScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (loadScene.activeInHierarchy == true)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
