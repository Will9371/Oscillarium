using UnityEngine;

namespace Playcraft
{
    public class FollowMouseOnScreenMono : MonoBehaviour
    {
        [SerializeField] FollowMouseOnScreen process = new FollowMouseOnScreen();
        void Start() { process.RefreshBounds(); }
        void Update() { transform.position = process.Update(); }
        void OnValidate() { process.RefreshBounds(); }
    }
}
