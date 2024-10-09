using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : GameBehaviour
{
    public GameObject loadScene;
    public string scene;

    public void LoadScene(string _sceneName) => SceneManager.LoadScene(_sceneName);
    public void ReloadScene() => LoadScene(SceneManager.GetActiveScene().name);
    public void LoadTitle() => LoadScene("TitleScene");
    public void QuitGame() => Application.Quit();

    private void Update()
    {
        if (loadScene.activeInHierarchy == true)
        {
            SceneManager.LoadScene(scene);
        }
        else
            return;
    }
}
