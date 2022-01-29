using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Hell/Bullet")]
public class BulletInfo : ScriptableObject
{
    public BulletData[] data;
    public int length => data.Length;
}

[Serializable]
public struct BulletData
{
    public float speed;
    public float yAmplitude;
    public float yFrequency;
    public AnimationCurve yCurve;
    public float xAmplitude;
    public float xFrequency;
    public AnimationCurve xCurve;
    public float reverseAfterSeconds;
    public float maxLifetime;
    public bool dontDestroyOffscreen;
    [Tooltip("Deactivates bullet. No effect if set to 0.")]
    public float destroyAfterSeconds;
}
