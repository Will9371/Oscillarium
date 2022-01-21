using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Hell/Bullet")]
public class BulletSO : ScriptableObject
{
    public BulletData data;
}

[Serializable]
public struct BulletData
{
    public Faction faction;
    public float speed;
    public float yAmplitude;
    public float yFrequency;
    public AnimationCurve yCurve;
    public bool reverseSine;
    public float reverseAfterSeconds;
    public float maxLifetime;
    public bool dontDestroyOffscreen;
    public float destroyAfterSeconds;
}
