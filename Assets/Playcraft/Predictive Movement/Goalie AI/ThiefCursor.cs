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
            InvokeRepeating(nameof(Refresh), 0.25f, 0.25f);
        }
        
        void Refresh() { bounds.screenFollow.RefreshBounds(); }
        
        void OnDestroy() { CancelInvoke(nameof(Refresh)); }
    }
}