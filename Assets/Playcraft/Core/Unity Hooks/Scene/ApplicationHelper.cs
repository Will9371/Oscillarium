using UnityEngine;
using UnityEngine.Events;

namespace Playcraft
{
    public class ApplicationHelper : MonoBehaviour
    {
        [SerializeField] UnityEvent OnQuit;
        public void Quit() { StaticHelpers.QuitGame(); }
        public void OnApplicationQuit() { OnQuit.Invoke(); }
    }
}