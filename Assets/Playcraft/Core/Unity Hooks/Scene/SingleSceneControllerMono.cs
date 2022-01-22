using UnityEngine;

// MODIFIED
namespace Playcraft.Scene
{
    public class SingleSceneControllerMono : MonoBehaviour
    {
        SingleSceneController process = new SingleSceneController();
    
        public void SetScene(StringSO value) { process.SetScene(value); }
        public void SetScene(string value) { process.SetScene(value); }
        public void ResetScene() { process.ResetScene(); }
        public void LoadNextLevel() { process.LoadNextLevel(); }
    }
}

