using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [Tooltip("In seconds")]
    [SerializeField] float totalTime;
    [SerializeField] Text timeText;
    [SerializeField] UnityEvent onTimeUp;
    [SerializeField] float timeUpDelay = 2f;
    
    float timeRemaining => totalTime - elapsedTime - skipTime;
    float elapsedTime => Time.time - startTime;
    float startTime;
    float skipTime;

    public void Begin(float skipTime = 0f)
    {
        this.skipTime = skipTime;
        startTime = Time.time;
        RefreshDisplay();
        InvokeRepeating(nameof(RefreshDisplay), 0.1f, 0.1f);
    }

    void Tick() { RefreshDisplay(); }
    
    TimeSpan timeSpan;
    
    void RefreshDisplay()
    {
        timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timeText.text = $"{timeSpan.Minutes:d1}:{timeSpan.Seconds:d2}";
        if (timeRemaining <= -0f) End();
    }
    
    void End() 
    { 
        CancelInvoke(nameof(RefreshDisplay)); 
        Invoke(nameof(OnTimeUp), timeUpDelay);
    }
    
    void OnTimeUp() { onTimeUp.Invoke(); }
}
