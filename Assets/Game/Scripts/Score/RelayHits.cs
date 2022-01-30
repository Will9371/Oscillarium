using UnityEngine;

public class RelayHits : MonoBehaviour
{
    [SerializeField] FloatEvent score;
    void Start() { GameData.onHitsChanged += OnHitsChanged; }
    void OnDestroy() { GameData.onHitsChanged -= OnHitsChanged; }
    void OnHitsChanged(float value) { score.Invoke(value); }
    public void GetValue() { score.Invoke(GameData.hits); }
}
