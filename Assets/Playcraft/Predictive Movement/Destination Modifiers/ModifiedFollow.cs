using UnityEngine;

namespace Playcraft
{
    public class ModifiedFollow : MonoBehaviour
    {
        [SerializeField] Transform other;
        [SerializeField] DestinationModifiers modifiers;
        void FixedUpdate() { transform.position = modifiers.Tick(other.position); }
    }
}