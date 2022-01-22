using System;
using UnityEngine;

namespace Playcraft.PredictiveMovement
{
    [CreateAssetMenu(menuName = "Playcraft/Predictive Movement/AI Settings")]
    public class GoalieAIInfo : ScriptableObject
    {
        public GoalieAIData data;
    }
    
    [Serializable]
    public struct GoalieAIData
    {
        public float speed;
        public bool stayInCircle;
        public float radius;
        public DestinationModifiers destinationModifiers;
    }
}
