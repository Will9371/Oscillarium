using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Hell/Bullet Spawner")]
public class BulletSpawnerInfo : ScriptableObject
{
    public BulletSpawnerData data;
}

[Serializable]
public struct BulletSpawnerData
{
    [Header("MultiDirectional Controls")]
    [Range(1, 10)] public float multiBullets;
    [Range(0,360)] public int multiDirectionalWidth;
    [Space]

    [Header("Cone Control")]
    [Range(1, 20)] public float coneBullets;
    [Range(0, 180)] public float coneWidth;
    [Space]

    [Header("Spin & Offset")]
    [Range(0, 360)] public float spinSpeed;
    public bool reverseSpin;
    public float offsetX;
    public float offsetY;
    [Space]

    [Header("Sweep Controls")]
    [Range(0, 180)] public float sweepAngle;
    [Range(0, 360)] public float sweepSpeed;
    [Space]

    [Header("Bullet Controls")]
    public BulletInfo bulletInfo;

    [Range(.01f, 2)] public float timeBetweenBullets;
    [Range(0.25f, 5)] public float bulletSizeX;
    [Range(0.25f, 5)] public float bulletSizeY;
    
    [Header("Variable Speeds")]
    public float bulletsBeforeRepeat;
    public float maxSpeedVariance;
    
    [Header("Stop Bullets")]
    public float stopAfterSeconds;
    public bool connectToSpawnerOnStop;
    [Space]

    [Header("Color Controls")]
    [Range(0, 100)] public float colorChangeSpeed;
    [Range(2, 3)] public float numberOfColors;
    public bool randomColorOrder;
    public bool startWithRandomColor;

    [Header("Rapid Fire Controls")]
    public int numberOfRapidFireBullets;
    [Range(0.1f, 30)] public float rapidFireCooldownTime;
    public float rapidFiresBeforeStop;    
}

