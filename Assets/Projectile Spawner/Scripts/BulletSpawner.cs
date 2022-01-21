using System.Collections.Generic;
using UnityEngine;
using Playcraft.Pooling;

public enum Faction { Player, Enemy }

public class BulletSpawner : MonoBehaviour
{
    ObjectPoolMaster pool => ObjectPoolMaster.instance;
    
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Faction faction = Faction.Enemy;

    public bool startFiring = false;
    [SerializeField] private bool stopAfterAllDestroyed = false;
    [Space]

    [Header("MultiDirectional Controls")]
    [SerializeField] [Range(1, 10)] private float multiBullets = 1;
    [SerializeField] [Range(0,360)] private int multiDirectionalWidth = 360;
    [Space]

    [Header("Cone Control")]
    [SerializeField] [Range(1, 20)] private float coneBullets = 1;
    [SerializeField] [Range(0, 180)] private float coneWidth = 45;
    [Space]

    [Header("Spin & Offset")]
    [SerializeField] [Range(0, 360)] float spinSpeed = 0;
    [SerializeField] bool reverseSpin = false;
    [SerializeField] float offsetX = 0;
    [SerializeField] float offsetY = 0;
    [Space]

    [Header("Sweep Controls")]
    [SerializeField] [Range(0, 180)] private float sweepAngle = 0;
    [SerializeField] [Range(0, 360)] private float sweepSpeed = 0;
    [Space]

    [Header("Bullet Controls")]
    [SerializeField] BulletInfo bulletInfo;
    
    [SerializeField] [Range(.01f, 2)] private float timeBetweenBullets = 0.5f;
    [SerializeField] [Range(0.25f, 5)] private float bulletSizeX = 1;
    [SerializeField] [Range(0.25f, 5)] private float bulletSizeY = 1;
    [Header("Reverse")]
    [Header("Variable Speeds")]
    [SerializeField] private float bulletsBeforeRepeat = 1;
    [SerializeField] private float maxSpeedVariance = 0;
    [Header("Stop Bullets")]
    [SerializeField] private float stopAfterSeconds = 0;
    [SerializeField] private bool connectToSpawnerOnStop = false;
    [Space]

    [Header("Color Controls")]
    [SerializeField] [Range(0, 100)] private float colorChangeSpeed = 0;
    [SerializeField] [Range(2, 3)] private float numberOfColors = 2;
    [Range(0,2)] public int bulletColorIndex = 0;
    [SerializeField] private bool randomColorOrder = false;
    [SerializeField] private bool startWithRandomColor = false;

    [Header("Rapid Fire Controls")]
    [SerializeField] private int numberOfRapidFireBullets = 1;
    [SerializeField] [Range(0.1f, 30)]private float rapidFireCooldownTime = 0.1f;
    [SerializeField] private float rapidFiresBeforeStop = 0;

    //hidden variables
    private Transform spawnerChild;
    private Transform spawner;
    private float nextFire = 0.0f;
    private float nextRapidFire = 0.0f;
    private float rapidFireBulletCount = 0;
    private float rapidFireArrayCount = 0;
    private float spinRotX;
    private float sweepRotX;
    private bool sweep = false;
    private float sweepPosition = 0;
    private float colorPosition = 0;
    private float speedVarianceCounter = 1;
    private float bulletsLifetime = 0;
    private bool freeze = false;
    
    public List<GameObject> bulletObjects = new List<GameObject>();
    public List<Bullet> bullets = new List<Bullet>();
    
    
    private void OnEnable()
    {
        spawner = transform;
        spawnerChild = transform.GetChild(0);
        
        if (startWithRandomColor)
            bulletColorIndex = Random.Range(0, 2);
    }
    
    private void Update()
    {
        bulletsLifetime += Time.deltaTime;
        if (!startFiring) return;
        FireBullets();
        SpinChild();
        Sweep();
        AutoColorChange();
        FreezeBullets();
        StopFiring();
    }
    
    private void SetupNewBullet(Quaternion rotation, BulletData data)
    {
        var bulletPosition = transform.position + new Vector3(0, offsetY, offsetX);
        GameObject bulletObject = pool.Spawn(bulletPrefab, bulletPosition, rotation);
        bulletObjects.Add(bulletObject);

        bulletObject.transform.localScale = new Vector3(bulletSizeX, bulletSizeY, bulletSizeX);
        bulletObject.GetComponent<ColorChange>().colorIndex = bulletColorIndex;
        
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullets.Add(bullet);
        
        //bulletData.speed = Mathf.Lerp(0, maxSpeedVariance, speedVarianceCounter / bulletsBeforeRepeat);
        bullet.Initialize(data, faction);

        // Adjust speeds of the bullets
        if (bulletsBeforeRepeat != 0) speedVarianceCounter++;
        if (speedVarianceCounter >= bulletsBeforeRepeat) speedVarianceCounter = 0;
    }
    
    private void FireBullets()
    {
        if (freeze || nextRapidFire > Time.time || nextFire > Time.time) return;
        if (numberOfRapidFireBullets > 1 && rapidFireBulletCount >= numberOfRapidFireBullets)
        {
            nextRapidFire = Time.time + rapidFireCooldownTime;
            rapidFireBulletCount = 0;
            rapidFireArrayCount++;
        }
        nextFire = Time.time + timeBetweenBullets;
        ShootMultiDirectional();
    }
    
    private void SpinChild()
    {
        if (spinSpeed == 0) return;
        float spinDirection = reverseSpin ? -1f : 1f;
        spinRotX += spinSpeed * spinDirection * Time.deltaTime;
        spawnerChild.rotation = Quaternion.Euler(spinRotX, 0, 0);
    }
    
    private void ShootMultiDirectional()
    {
        if (numberOfRapidFireBullets > 1)
            rapidFireBulletCount++;

        for (int i = 0; i <= multiBullets-1; i++)
        {
            float width = Mathf.Lerp(0, multiDirectionalWidth, i/multiBullets);
            if (multiBullets == 1) width = 0;
            ShootCone(width);
        }
    }
    
    private void ShootCone(float multiWidth)
    {
        for (int i = 0; i <= coneBullets-1; i++)
        {
            float width = Mathf.Lerp(-coneWidth / 2, coneWidth / 2, i / (coneBullets - 1));
            if (coneBullets == 1) width = 0;
            
            Quaternion bulletRotation;
            foreach (var bulletData in bulletInfo.data)
            {
                Debug.Log(bulletInfo.length);
                bulletRotation = spawnerChild.transform.rotation * Quaternion.Euler(width + multiWidth, 0, 0);
                SetupNewBullet(bulletRotation, bulletData);
            }
        }
    }
    
    private void Sweep()
    {
        if (sweepSpeed == 0) return;
        float angleDivisor = sweep ? 2f : 1f;
        float sweepDirection = sweep ? -1f : 1f;
        sweepRotX = Mathf.Lerp(-sweepAngle/angleDivisor, sweepAngle/angleDivisor, sweepPosition/100);
        sweepPosition += sweepSpeed * sweepDirection * Time.deltaTime;
        if (sweep && sweepPosition <= 0) sweep = false;
        else if (!sweep && sweepPosition >= 100) sweep = true;
        spawner.rotation = Quaternion.Euler(sweepRotX, 0, 0);
    }
    
    private void AutoColorChange()
    {
        if (colorChangeSpeed <= 0) return;
        if(colorPosition >= 10)
        {
            colorPosition = 0;
            ChangeColor();
        }
        colorPosition += Time.deltaTime * colorChangeSpeed;
    }
    
    private void ChangeColor()
    {
        if (!randomColorOrder)
        {
            bulletColorIndex = bulletColorIndex == numberOfColors - 1 ? 0 : bulletColorIndex + 1;
            return;
        }
        bulletColorIndex = Random.Range(0, Mathf.RoundToInt(numberOfColors));
    }
    
    private void FreezeBullets()
    {
        if (stopAfterSeconds == 0 || bulletsLifetime <= stopAfterSeconds) 
            return;

        foreach (var bullet in bullets)
        {
            bullet.data.speed = 0f;
            freeze = true;
            if (!connectToSpawnerOnStop) return;
            if (!bullet) return;
            bullet.transform.SetParent(transform.GetChild(0));
        }
    }
    
    private void StopFiring()
    {
        if (nextFire == 0) return;
        if (rapidFiresBeforeStop <= rapidFireArrayCount & rapidFiresBeforeStop > 0) Destroy(gameObject);
        if (stopAfterAllDestroyed && AllBulletsOffscreen()) DestroyAll();
        if (stopAfterSeconds <= 0) return;
        if (stopAfterSeconds <= bulletsLifetime) startFiring = false;
    }

    private bool AllBulletsOffscreen()
    {
        foreach (var bullet in bulletObjects)
            if (bullet.activeSelf)
                return false;
        
        return true;
    }
    
    private void DestroyAll()
    {
        for (int i = bulletObjects.Count - 1; i >= 0; i--)
        {
            bulletObjects.Remove(bulletObjects[i]);
            bullets.Remove(bullets[i]);
        }
        
        Destroy(gameObject);
    }
}
