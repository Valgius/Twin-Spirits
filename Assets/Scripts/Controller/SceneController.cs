using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneController : GameBehaviour
{
    public VideoPlayer cutscene;
    public GameObject loadScene;
    public string scene;

    public void LoadScene(string _sceneName) => SceneManager.LoadScene(_sceneName);
    public void ReloadScene() => LoadScene(SceneManager.GetActiveScene().name);
    public void LoadTitle() => LoadScene("TitleScene");
    public void QuitGame() => Application.Quit();

    private void Start()
    {
        Time.timeScale = 1;
        if(cutscene != null)
        {
            cutscene.Play();
            cutscene.loopPointReached += EndCutscene;
            
        }
        
    }

    private void Update()
    {

        if (loadScene != null && loadScene.activeInHierarchy == true)
        {
            SceneManager.LoadScene(scene);
        }
        else
            return;
    }

    void EndCutscene(VideoPlayer player)
    {
        loadScene.SetActive(true);
    }
}
