using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    private bool isPaused = false;

    public void TogglePause() { SetPaused(!isPaused); }
    
    public void SetPaused(bool value)
    {
        isPaused = value;
        Time.timeScale = isPaused ? 0 : 1;
        PauseMenu.SetActive(isPaused);        
    }
}