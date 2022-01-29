using System;
using UnityEngine;

namespace Playcraft.PredictiveMovement
{
    [Serializable]
    public class GoalieAI
    {
        [Header("References")]
        public Transform self;
        public Transform target;
        public Transform center;
        
        public GoalieAIInfo info;
        
        float speed => info.data.speed;
        public bool stayInCircle => info.data.stayInCircle;
        public float radius => info.data.radius;
        public DestinationModifiers destinationModifiers => info.data.destinationModifiers;

        FollowOnCircle bounds = new FollowOnCircle();
        
        public void Initialize() 
        {
            bounds.radius = radius;
            if (center) bounds.center = center.position;
        }
        
        Vector3 targetPosition;
        public Vector3 direction { get; private set; }

        public void FixedUpdate() 
        {
            targetPosition = destinationModifiers.Tick(target.position);
            if (stayInCircle) targetPosition = bounds.Update(targetPosition);
            
            direction = targetPosition - self.position;
            direction = direction.normalized;
            
            self.Translate(speed * Time.fixedDeltaTime * direction);
        }
    }
}
