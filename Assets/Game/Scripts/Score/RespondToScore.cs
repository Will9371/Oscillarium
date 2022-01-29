using System;
using UnityEngine;
using UnityEngine.Events;

public class RespondToScore : MonoBehaviour
{
    [SerializeField] Binding[] bindings;
    
    bool hasRequiredPoints;

    void Start()
    {
        GameData.onScoreChanged += OnScoreChanged;
    } 

    void OnDestroy()
    {
        GameData.onScoreChanged -= OnScoreChanged;
    }
    
    void OnScoreChanged(float value)
    {
        for (int i = 0; i < bindings.Length; i++)
        {
            if (!bindings[i].reached && value >= bindings[i].threshold)
            {
                bindings[i].reached = true;
                bindings[i].response.Invoke();
            }
        }
    }
    
    [Serializable]
    public struct Binding
    {
        public float threshold;
        [HideInInspector] public bool reached;
        public UnityEvent response;
    }
}
