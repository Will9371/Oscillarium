using System;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [Tooltip("In seconds")]
    [SerializeField] float totalTime;
    [SerializeField] Text timeText;
    
    float timeRemaining => totalTime - elapsedTime - skipTime;
    float elapsedTime => Time.time - startTime;
    float startTime;
    float skipTime;

    public void Begin(float skipTime = 0f)
    {
        this.skipTime = skipTime;
        startTime = Time.time;
        RefreshDisplay();
        InvokeRepeating(nameof(Tick), 0.1f, 0.1f);
    }
    
    public void End() { CancelInvoke(nameof(Tick)); }
    
    void Tick() { RefreshDisplay(); }
    
    TimeSpan timeSpan;
    
    void RefreshDisplay()
    {
        timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timeText.text = $"{timeSpan.Minutes:d1}:{timeSpan.Seconds:d2}";
    }
}
