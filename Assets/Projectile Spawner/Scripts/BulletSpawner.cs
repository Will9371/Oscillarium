using System.Collections.Generic;
using UnityEngine;
using Playcraft.Pooling;

// REFACTOR: break into sections, then delegate to helper classes
// Also break info into sub-structs, associated with helper classes
public class BulletSpawner : MonoBehaviour
{
    ObjectPoolMaster pool => ObjectPoolMaster.instance;
    Lookup lookup => Lookup.instance;
    GameObject bulletPrefab => lookup.bulletPrefab;
    
    [SerializeField] BulletSpawnerInfo info;
    BulletSpawnerData data => info.data;

    [SerializeField] bool startFiring;
    public bool isFiring;
    [SerializeField] bool stopAfterAllDestroyed;
    [Range(0,2)] public int bulletColorIndex;

    Transform spawnerChild;
    Transform spawner;
    float nextFire;
    float nextRapidFire;
    float rapidFireBulletCount;
    float rapidFireArrayCount;
    float spinAngle;
    float sweepAngle;
    bool sweep;
    float sweepPosition;
    float colorPosition;
    float speedVarianceCounter = 1;
    float bulletsLifetime;
    bool freeze;
    
    public List<GameObject> bulletObjects = new List<GameObject>();
    public List<Bullet> bullets = new List<Bullet>();
    
    
    void OnEnable()
    {
        spawner = transform;
        spawnerChild = transform.GetChild(0);
        isFiring = startFiring;
        
        if (data.startWithRandomColor)
            bulletColorIndex = Random.Range(0, 2);
    }
    
    void Update()
    {
        bulletsLifetime += Time.deltaTime;
        if (!isFiring) return;
        
        FireBullets();
        SpinChild();
        Sweep();
        AutoColorChange();
        FreezeBullets();
        StopFiring();
    }
    
    void SetupNewBullet(Quaternion rotation, BulletData bulletData)
    {
        var bulletPosition = transform.position + new Vector3(0, data.offsetY, data.offsetX);
        GameObject bulletObject = pool.Spawn(bulletPrefab, bulletPosition, rotation);
        bulletObjects.Add(bulletObject);

        bulletObject.transform.localScale = new Vector3(data.bulletSizeX, data.bulletSizeY, data.bulletSizeX);
        bulletObject.GetComponent<ColorChange>().SetColor(bulletColorIndex);
        
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullets.Add(bullet);
        
        //bulletData.speed = Mathf.Lerp(0, maxSpeedVariance, speedVarianceCounter / bulletsBeforeRepeat);
        bullet.Initialize(bulletData);

        // Adjust speeds of the bullets
        if (data.bulletsBeforeRepeat != 0) speedVarianceCounter++;
        if (speedVarianceCounter >= data.bulletsBeforeRepeat) speedVarianceCounter = 0;
    }
    
    void FireBullets()
    {
        if (freeze || nextRapidFire > Time.time || nextFire > Time.time) return;
        if (data.numberOfRapidFireBullets > 1 && rapidFireBulletCount >= data.numberOfRapidFireBullets)
        {
            nextRapidFire = Time.time + data.rapidFireCooldownTime;
            rapidFireBulletCount = 0;
            rapidFireArrayCount++;
        }
        nextFire = Time.time + data.timeBetweenBullets;
        ShootMultiDirectional();
    }
    
    void SpinChild()
    {
        if (data.spinSpeed == 0) return;
        float spinDirection = data.reverseSpin ? -1f : 1f;
        spinAngle += data.spinSpeed * spinDirection * Time.deltaTime;
        spawnerChild.rotation = Quaternion.Euler(0, 0, spinAngle);
    }
    
    void ShootMultiDirectional()
    {
        if (data.numberOfRapidFireBullets > 1)
            rapidFireBulletCount++;

        for (int i = 0; i <= data.multiBullets - 1; i++)
        {
            float width = Mathf.Lerp(0, data.multiDirectionalWidth, i/data.multiBullets);
            if (data.multiBullets == 1) width = 0;
            ShootCone(width);
        }
    }
    
    void ShootCone(float multiWidth)
    {
        for (int i = 0; i <= data.coneBullets - 1; i++)
        {
            float width = Mathf.Lerp(-data.coneWidth / 2, data.coneWidth / 2, i / (data.coneBullets - 1));
            if (data.coneBullets == 1) width = 0;
            
            Quaternion bulletRotation;
            foreach (var bulletData in data.bulletInfo.data)
            {
                bulletRotation = spawnerChild.transform.rotation * Quaternion.Euler(0, 0, width + multiWidth);
                SetupNewBullet(bulletRotation, bulletData);
            }
        }
    }
    
    void Sweep()
    {
        if (data.sweepSpeed == 0) return;
        float angleDivisor = sweep ? 2f : 1f;
        float sweepDirection = sweep ? -1f : 1f;
        sweepAngle = Mathf.Lerp(-data.sweepAngle/angleDivisor, data.sweepAngle/angleDivisor, sweepPosition/100);
        sweepPosition += data.sweepSpeed * sweepDirection * Time.deltaTime;
        if (sweep && sweepPosition <= 0) sweep = false;
        else if (!sweep && sweepPosition >= 100) sweep = true;
        spawner.rotation = Quaternion.Euler(0, 0, sweepAngle);
    }
    
    void AutoColorChange()
    {
        if (data.colorChangeSpeed <= 0) return;
        if(colorPosition >= 10)
        {
            colorPosition = 0;
            ChangeColor();
        }
        colorPosition += Time.deltaTime * data.colorChangeSpeed;
    }
    
    void ChangeColor()
    {
        if (!data.randomColorOrder)
        {
            bulletColorIndex = bulletColorIndex == data.numberOfColors - 1 ? 0 : bulletColorIndex + 1;
            return;
        }
        bulletColorIndex = Random.Range(0, Mathf.RoundToInt(data.numberOfColors));
    }
    
    void FreezeBullets()
    {
        if (data.stopAfterSeconds == 0 || bulletsLifetime <= data.stopAfterSeconds) 
            return;

        foreach (var bullet in bullets)
        {
            bullet.data.speed = 0f;
            freeze = true;
            if (!data.connectToSpawnerOnStop) return;
            if (!bullet) return;
            bullet.transform.SetParent(transform.GetChild(0));
        }
    }
    
    void StopFiring()
    {
        if (nextFire == 0) return;
        if (data.rapidFiresBeforeStop <= rapidFireArrayCount & data.rapidFiresBeforeStop > 0) Destroy(gameObject);
        if (stopAfterAllDestroyed && AllBulletsOffscreen()) DestroyAll();
        if (data.stopAfterSeconds <= 0) return;
        if (data.stopAfterSeconds <= bulletsLifetime) isFiring = false;
    }

    bool AllBulletsOffscreen()
    {
        foreach (var bullet in bulletObjects)
            if (bullet.activeSelf)
                return false;
        
        return true;
    }
    
    void DestroyAll()
    {
        for (int i = bulletObjects.Count - 1; i >= 0; i--)
        {
            bulletObjects.Remove(bulletObjects[i]);
            bullets.Remove(bullets[i]);
        }
        
        Destroy(gameObject);
    }
}
