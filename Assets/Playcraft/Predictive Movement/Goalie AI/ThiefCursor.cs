using UnityEngine;

namespace Playcraft.PredictiveMovement
{
    public class ThiefCursor : MonoBehaviour
    {
        [SerializeField] FollowScreenBoundedMouseInWorld bounds;
        void Update() { transform.position = bounds.Update(); }
        void OnValidate() { bounds.OnValidate(); }
        
        SingleSceneController scene = new SingleSceneController();

        void Start() 
        { 
            if (bounds.camera == null) 
                bounds.camera = Camera.main; 
            
            Cursor.visible = false;
        }
        
        public void TouchThreat() 
        {
            Debug.Log("Oh no!");
            GameData.score = 0f;  
            scene.ResetScene(); 
        }
        
        bool hasWon;
        
        public void TouchTarget() 
        {
            if (!hasWon)
            {
                hasWon = true;
                Debug.Log("You Win!!!"); 
            }
        }
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