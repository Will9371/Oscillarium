using UnityEngine;

namespace Playcraft.PredictiveMovement
{
    public class ThiefCursor : MonoBehaviour
    {
        [SerializeField] FollowScreenBoundedMouseInWorld bounds;
        void Update() { transform.position = bounds.Update(); }
        void OnValidate() { bounds.OnValidate(); }
        
        void Start() 
        { 
            if (bounds.camera == null) 
                bounds.camera = Camera.main; 
            
            bounds.Start();
            Cursor.visible = false;
        }
    }
}