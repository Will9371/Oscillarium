using UnityEngine.SceneManagement;

// NEW
namespace Playcraft
{
    public class SingleSceneController
    {
        public void SetScene(StringSO value) { SetScene(value.value); }
        public void SetScene(string name) { SceneManager.LoadScene(name); }

        public void ResetScene()
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            SetScene(scene.name);
        }
            
        public void LoadNextLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }
    }
}
