using UnityEngine;

namespace Playcraft
{
    public abstract class DestinationModifier : ScriptableObject
    {
        public abstract Vector3 Tick(Vector3 priorDestination);
    }
}