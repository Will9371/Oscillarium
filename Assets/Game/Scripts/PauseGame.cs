using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    private bool isPaused = false;

    public void PauseKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           if (!isPaused)
           {
               PauseGameState();
           }
           else
           {
               ResumeGameState();
           }
        } 
    }

    public void PauseGameState()
    {
        Time.timeScale = 0;
        isPaused = true;
        PauseMenu.SetActive(true);
    }

    public void ResumeGameState()
    {
        Time.timeScale = 1;
        isPaused = false;
        PauseMenu.SetActive(false);
    }
}
