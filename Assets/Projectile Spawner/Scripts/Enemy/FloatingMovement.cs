using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0.03f;
    [SerializeField] private float sineAmplitude = 0.009f;
    [SerializeField] private float sineFrequency = 1000;
    [SerializeField] private float waitTimeBeforeMoving = 0;
    private float lifeTime = 0;
    private float sineTime = 0;
    private float yDirection = 0;
    private float sineWavePosition = 0;
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime < waitTimeBeforeMoving) return;
        transform.Translate(transform.forward * speed);
        if (sineAmplitude == 0) return;
        sineTime += yDirection;
        sineWavePosition = Mathf.Lerp(-sineAmplitude, sineAmplitude, sineTime / sineFrequency);
        transform.Translate(Vector3.up * sineWavePosition);
        if (sineTime >= sineFrequency) yDirection = -1;
        if (sineTime <= 0) yDirection = 1;
    }
}
