using UnityEngine;

public class SetScorePercent : MonoBehaviour
{
    [SerializeField] FloatEvent Percent;

    float correct;
    float incorrect;
    float total => correct + incorrect;
    float percent => total <= 0 ? 0 : correct/total;

    public void RefreshCorrect(float value)
    {
        correct = value;
        Percent.Invoke(percent);
    }
    
    public void RefreshIncorrect(float value)
    {
        incorrect = value;
        Percent.Invoke(percent);
    }
    
    public void GetValue()
    {
        correct = GameData.score;
        incorrect = GameData.hits;
        Percent.Invoke(percent);
    }
}
