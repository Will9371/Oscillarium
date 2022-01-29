using UnityEngine;

public class RelayScore : MonoBehaviour
{
    [SerializeField] FloatEvent score;
    void Start() { GameData.onScoreChanged += OnScoreChanged; }
    void OnDestroy() { GameData.onScoreChanged -= OnScoreChanged; }
    void OnScoreChanged(float value) { score.Invoke(value); }
}
