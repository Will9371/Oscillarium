using System.Collections;
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
    ColorInfo colorInfo => lookup.colorInfo;
    int colorCount => lookup.colorCount;
    
    public BulletSpawnerInfo info;
    BulletSpawnerData data => info.data;

    [SerializeField] bool startFiring;
    public bool isFiring;
    [SerializeField] bool stopAfterAllDestroyed;
    [Range(0,2)] public int bulletColorIndex;

    // * Remove, condense baseAngle and baseChildAngle
    Transform spawnerChild;
    Transform spawner;
    float nextRapidFire;
    float rapidFireBulletCount;
    float rapidFireArrayCount;
    
    float baseChildAngle;
    float spinAngle;
    float baseAngle;
    float sweepAngle;
    
    /// Sets sweep system active/inactive
    bool sweep;
    float sweepPosition;    // * REFACTOR: the pattern this uses is insane
    bool freeze;
    
    public List<GameObject> bulletObjects = new List<GameObject>();
    public List<Bullet> bullets = new List<Bullet>();
    
    
    void OnEnable()
    {
        spawner = transform;
        spawnerChild = transform.GetChild(0);
        isFiring = startFiring;
        spinAngle = 0;
        sweepAngle = 0;
        freeze = false;
        sweep = false;
        sweepPosition = 0f;

        spawnerChild.localRotation = Quaternion.identity;
        baseAngle = transform.rotation.eulerAngles.z;
        baseChildAngle = spawnerChild.rotation.eulerAngles.z;

        StartCoroutine(FireRoutine());
        StartCoroutine(ChangeColorRoutine());
        
        if (data.stopAfterSeconds > 0)
            Invoke(nameof(FreezeBullets), data.stopAfterSeconds);
        
        if (stopAfterAllDestroyed)
            InvokeRepeating(nameof(StopIfAllBulletsOffscreen), .2f, .2f);
    }
    
    void OnDisable()
    {
        StopAllCoroutines();
    }
    
    void Update()
    {
        if (!isFiring) return;
        SpinChild();
        Sweep();
    }
    
    void ShootBullet(Quaternion rotation, BulletData bulletData)
    {
        var bulletPosition = transform.position + new Vector3(0, data.offsetY, data.offsetX);
        Debug.Log(pool == null);
        Debug.Log(bulletPrefab == null);
        var bulletObject = pool.Spawn(bulletPrefab, bulletPosition, rotation);
        bulletObjects.Add(bulletObject);

        bulletObject.transform.localScale = new Vector3(data.bulletSizeX, data.bulletSizeY, data.bulletSizeX);
        bulletObject.GetComponent<ColorChange>().SetColor(bulletColorIndex);
        
        var bullet = bulletObject.GetComponent<Bullet>();
        bullets.Add(bullet);
        bullet.Initialize(bulletData);
    }

    IEnumerator FireRoutine()
    {
        var delay = new WaitForSeconds(data.timeBetweenBullets);
        
        while (true)
        {
            ShootMultiDirectional();
            yield return delay;
        }
    }
    
    //if (data.numberOfRapidFireBullets > 1 && rapidFireBulletCount >= data.numberOfRapidFireBullets)
    //{
    //    nextRapidFire = Time.time + data.rapidFireCooldownTime;
    //    rapidFireBulletCount = 0;
    //    rapidFireArrayCount++;
    //}  
    
    //if (data.rapidFiresBeforeStop <= rapidFireArrayCount & data.rapidFiresBeforeStop > 0) gameObject.SetActive(false);  

    void SpinChild()
    {
        if (data.spinSpeed == 0) return;
        float spinDirection = data.reverseSpin ? -1f : 1f;
        spinAngle += data.spinSpeed * spinDirection * Time.deltaTime;
        spawnerChild.rotation = Quaternion.Euler(0, 0, spinAngle + baseChildAngle);
    }
    
    void ShootMultiDirectional()
    {
        if (data.numberOfRapidFireBullets > 1)
            rapidFireBulletCount++;

        for (int i = 0; i <= data.multiBullets - 1; i++)
        {
            float width = data.multiBullets <= 1 ? 0 : Mathf.Lerp(0, data.multiDirectionalWidth, i/(float)data.multiBullets);
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
                ShootBullet(bulletRotation, bulletData);
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
        spawner.rotation = Quaternion.Euler(0, 0, sweepAngle + baseAngle);
    }

    IEnumerator ChangeColorRoutine()
    {
        if (data.startWithRandomColor)
            bulletColorIndex = Random.Range(0, colorCount);
    
        while (true)
        {
            var waitTime = Random.Range(data.colorChangeTimeRange.x, data.colorChangeTimeRange.y);
            yield return new WaitForSeconds(waitTime);
            ChangeColor();
        }
    }
    
    void ChangeColor()
    {
        if (!data.randomColorOrder)
        {
            bulletColorIndex = bulletColorIndex == colorCount - 1 ? 0 : bulletColorIndex + 1;
            return;
        }
        bulletColorIndex = Random.Range(0, Mathf.RoundToInt(colorCount));
    }
    
    void FreezeBullets()
    {
        StopCoroutine(nameof(FireRoutine));

        foreach (var bullet in bullets)
        {
            bullet.data.speed = 0f;
            if (!data.connectToSpawnerOnStop) return;
            if (!bullet) return;
            bullet.transform.SetParent(transform.GetChild(0));
        }
    }
    
    void StopIfAllBulletsOffscreen() { if (AllBulletsOffscreen()) DestroyAll(); }

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
        
        gameObject.SetActive(false);
    }
}
