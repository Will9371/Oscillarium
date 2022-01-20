using System.Collections.Generic;
using UnityEngine;
using Playcraft.Pooling;

public enum Faction { Player, Enemy }

public class BulletSpawner : MonoBehaviour
{
    ObjectPoolMaster pool => ObjectPoolMaster.instance;
    
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Faction faction;

    [SerializeField] private Transform bulletTransform = null;
    public bool startFiring = false;
    [SerializeField] private bool stopAfterAllDestroyed = false;
    public bool doNotDestroyOffScreen = false;
    public float timeNeededBeforeMoving = 0;
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
    [SerializeField] [Range(0, 200)] private float bulletSpeed = 100f;
    [SerializeField] [Range(0.01f, 2)] private float timeBetweenBullets = 0.5f;
    [SerializeField] [Range(0.25f, 5)] private float bulletSizeX = 1;
    [SerializeField] [Range(0.25f, 5)] private float bulletSizeY = 1;
    [Header("SineWave")]
    [SerializeField] private bool doubleSine = false;
    [SerializeField] private float yAmplitude = 0;
    [SerializeField] private float yFrequency = 0;
    [Header("Reverse")]
    [SerializeField] private float reverseAfterSeconds = 0;
    [Header("Variable Speeds")]
    [SerializeField] private float bulletsBeforeRepeat = 1;
    [SerializeField] private float maxSpeedVariance = 0;
    [Header("Stop Bullets")]
    [SerializeField] private float stopAfterSeconds = 0;
    [SerializeField] private bool connectToSpawnerOnStop = false;
    [Header("TimeDestroyBullets")]
    [SerializeField] public float destroyAfterSeconds = 0;
    [Space]

    [Header("Color Controls")]
    [SerializeField] [Range(0, 100)] private float colorChangeSpeed = 0;
    [SerializeField] [Range(2, 3)] private float numberOfColors = 2;
    [SerializeField] [Range(0,2)] private int bulletColor = 0;
    [SerializeField] private bool randomColorOrder = false;
    [SerializeField] private bool startWithRandomColor = false;

    [Header("Rapid Fire Controls")]
    [SerializeField] private int numberOfRapidFireBullets = 1;
    [SerializeField] [Range(0.1f, 30)]private float rapidFireCooldownTime = 0.1f;
    [SerializeField] private float rapidFiresBeforeStop = 0;

    //hidden variables
    private Bullet bullet = null;
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
    public List<GameObject> bulletsCreated;
    private float bulletsLifetime = 0;
    private bool freeze = false;
    //private bool isEnemy = false;

    // WPP: naked passthrough creates an illusion of encapsulation, just use bulletColor
    // + rename: bulletColorIndex
    public int BulletColor { private get => bulletColor; set => bulletColor = value; }
    
    private void OnEnable()
    {
        //if (gameObject.CompareTag("Enemy")) isEnemy = true;
        spawner = transform;
        spawnerChild = transform.GetChild(0);
        bullet = bulletTransform.GetComponent<Bullet>();
        
        if (startWithRandomColor)
            BulletColor = Random.Range(0, 2);
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
    
    private void SetupNewBullet(Quaternion rotation, bool reverseSine)
    {
        var bulletPosition = transform.position + new Vector3(0, offsetY, offsetX);
        GameObject newBullet = pool.Spawn(bulletPrefab, bulletPosition, rotation);

        // Assign variables to bullet
        newBullet.transform.localScale = new Vector3(bulletSizeX, bulletSizeY, bulletSizeX);
        newBullet.GetComponent<ColorChange>().color = BulletColor;
        Bullet bullet = newBullet.GetComponent<Bullet>();
        bullet.bulletSpawner = this;
        bullet.speed = bulletSpeed + Mathf.Lerp(0, maxSpeedVariance, speedVarianceCounter / bulletsBeforeRepeat);
        bullet.yAmplitude = yAmplitude;
        bullet.yFrequency = yFrequency;
        bullet.reverseSine = reverseSine;
        bullet.reverseAfterSeconds = reverseAfterSeconds;
        bullet.speed = bulletSpeed;
        //bulletControl.isEnemy = isEnemy;
        bullet.faction = faction;

        // Adjust speeds of the bullets
        if (bulletsBeforeRepeat != 0) speedVarianceCounter++;
        if (speedVarianceCounter >= bulletsBeforeRepeat) speedVarianceCounter = 0;
    }
    
    private void FireBullets()
    {
        if (freeze) return;
        if (nextRapidFire > Time.time) return;
        if (nextFire > Time.time) return;
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
            float width = Mathf.Lerp(0, multiDirectionalWidth, i/(multiBullets));
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
            if (doubleSine)
            {
                SetupNewBullet(spawnerChild.transform.rotation * Quaternion.Euler(width + multiWidth, 0, 0), true);
            }
            SetupNewBullet(spawnerChild.transform.rotation * Quaternion.Euler(width + multiWidth, 0, 0), false);
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
            BulletColor = BulletColor == numberOfColors - 1 ? 0 : BulletColor + 1;
            return;
        }
        BulletColor = Random.Range(0, Mathf.RoundToInt(numberOfColors));
    }
    
    private void FreezeBullets()
    {
        if (stopAfterSeconds == 0 || bulletsLifetime <= stopAfterSeconds) 
            return;

        foreach (var bullet in bulletsCreated)
        {
            bullet.GetComponent<Bullet>().speed = 0;
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
        foreach (var bullet in bulletsCreated)
            if (bullet.activeSelf)
                return false;
        
        return true;
    }
    
    private void DestroyAll()
    {
        for(int i = 0; i < bulletsCreated.Count; i++)
            bulletsCreated.Remove(bulletsCreated[i]);
        
        Destroy(gameObject);
    }
}
