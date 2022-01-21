using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonActions : MonoBehaviour
{
    public void LoadLevelScene()
    {
        if(SceneManager.GetSceneByName("LevelsMenu") == null)
        {
            Debug.Log("LevelsMenuNotFound");
            return;
        }
        SceneManager.LoadScene("LevelsMenu");
    }

    public void LoadMainMenuScene()
    {
        if (SceneManager.GetSceneByName("MainMenu") == null)
        {
            Debug.Log("MainMenuNotFound");
            return;
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadToyboxLevel()
    {
        SceneManager.LoadScene("Toybox");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
