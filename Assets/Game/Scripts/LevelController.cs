using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    [SerializeField] float pointsRequired;
    [SerializeField] UnityEvent onReachPointsRequired;
    
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
        if (!hasRequiredPoints && value >= pointsRequired)
        {
            hasRequiredPoints = true;
            onReachPointsRequired.Invoke();
        }
    }
}
