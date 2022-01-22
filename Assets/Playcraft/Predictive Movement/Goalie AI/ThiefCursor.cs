using UnityEngine;
using Playcraft;

namespace Playcraft.PredictiveMovement
{
    public class ThiefCursor : MonoBehaviour
    {
        [SerializeField] FollowScreenBoundedMouseInWorld bounds;
        void Update() { transform.position = bounds.Update(); }
        void OnValidate() { bounds.OnValidate(); }
        
        SingleSceneController scene = new SingleSceneController();
        
        /*public RespondToCustomTag pickups;
        public RespondToCustomTag threats;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (threats.Input(other))
                scene.ResetScene();
            else if (pickups.Input(other))
                Debug.Log("Point!");
        }*/
        
        void Start() 
        { 
            if (bounds.camera == null) 
                bounds.camera = Camera.main; 
        }
        
        public void TouchThreat() { Debug.Log("Reset!"); scene.ResetScene(); }
        
        public void TouchTarget() { Debug.Log("Point!"); }
    }
}


//public float cooldown = 0.5f;
//float lastCaughtTime;
//float lastStealTime;
        
//bool recentlyCaught => Time.time - lastCaughtTime < cooldown;
//bool recentlyStolen => Time.time - lastStealTime < cooldown;

/*void Caught()
{
    if (recentlyCaught) return;
    scene.ResetScene();
    lastCaughtTime = Time.time;
    Debug.Log("Caught!");
}*/
        
/*void Steal()
{
    if (recentlyCaught || recentlyStolen) return;
    lastStealTime = Time.time;
    Invoke(nameof(Verdict), cooldown);
}*/
        
/*void Verdict()
{
    if (recentlyCaught) return;
    Debug.Log("Steal!");
}*/