using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Transform bulletTransform = null;
    [SerializeField] public bool startFiring = false;
    [SerializeField] private bool stopAfterAllDestroyed = false;
    [SerializeField] public bool doNotDestroyOffScreen = false;
    [SerializeField] public float timeNeededBeforeMoving = 0;
    [Space]

    [Header("MultiDirectional Controls")]
    [SerializeField] [Range(1, 10)] private float multiBullets = 1;
    [SerializeField] [Range(0,360)] private int multiDirectionalWidth = 360;
    [Space]

    [Header("Cone Control")]
    [SerializeField] [Range(1, 20)]private float coneBullets = 1;
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
    [SerializeField] private float sineAmplitude = 0;
    [SerializeField] private float sineFrequency = 0;
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
    private bool isEnemy = false;

    public int BulletColor { private get => bulletColor; set => bulletColor = value; }
    private void OnEnable()
    {
        if (gameObject.tag == "Enemy") isEnemy = true;
        spawner = this.transform;
        spawnerChild = this.transform.GetChild(0);
        bullet = bulletTransform.GetComponent<Bullet>();
        if (startWithRandomColor)
        {
            BulletColor = Random.Range(0, 2);
        }
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
        //Setting Up Spawner
        GameObject newBullet = GetBulletFromPool();
        //Creating a new bullet if there is none
        if (newBullet == null)
        {
            newBullet = Instantiate(bullet.gameObject, spawnerChild.transform.position + new Vector3(0, offsetY, offsetX), rotation);
            bulletsCreated.Add(newBullet);
        }
        //Reusing Bullet from pool if possible
        else
        {
            newBullet.transform.SetPositionAndRotation(this.transform.position + new Vector3(0, offsetY, offsetX), rotation);
            newBullet.SetActive(true);
        }

        //Assigning variables to bullet
        newBullet.transform.localScale = new Vector3(bulletSizeX, bulletSizeY, bulletSizeX);
        newBullet.GetComponent<ColorChange>().color = BulletColor;
        Bullet bulletControl = newBullet.GetComponent<Bullet>();
        bulletControl.bulletSpawner = this;
        bulletControl.speed = bulletSpeed + Mathf.Lerp(0, maxSpeedVariance, speedVarianceCounter / bulletsBeforeRepeat);
        bulletControl.sineAmplitude = sineAmplitude;
        bulletControl.sineFrequency = sineFrequency;
        bulletControl.reverseSine = reverseSine;
        bulletControl.reverseAfterSeconds = reverseAfterSeconds;
        bulletControl.speed = bulletSpeed;
        bulletControl.isEnemy = isEnemy;

        //Adjusting the speeds of the bullets
        if (bulletsBeforeRepeat != 0) speedVarianceCounter++;
        if (speedVarianceCounter >= bulletsBeforeRepeat) speedVarianceCounter = 0;

    }
    private void FireBullets()
    {
        if (freeze == true) return;
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
        if (spinSpeed != 0)
        {
            if (reverseSpin)
            {
                spinRotX -= Time.deltaTime * spinSpeed;
            }
            else
            {
                spinRotX += Time.deltaTime * spinSpeed;
            }
            spawnerChild.rotation = Quaternion.Euler(spinRotX, 0, 0);
        }
    }
    private void ShootMultiDirectional()
    {
        if(numberOfRapidFireBullets > 1)
        {
            rapidFireBulletCount++;
        }
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
                SetupNewBullet(spawnerChild.transform.rotation * Quaternion.Euler((width + multiWidth), 0, 0), true);
            }
            SetupNewBullet(spawnerChild.transform.rotation * Quaternion.Euler((width + multiWidth), 0, 0), false);
        }
    }
    private void Sweep()
    {
        if (sweepSpeed != 0)
        {
            if (!sweep)
            {
                float width = Mathf.Lerp(-sweepAngle, sweepAngle, sweepPosition/100);
                sweepRotX = width;
                sweepPosition += Time.deltaTime * sweepSpeed;
                if (sweepPosition >= 100) sweep = true;
            }
            else
            {
                float width = Mathf.Lerp(-sweepAngle/2, sweepAngle/2, sweepPosition/100);
                sweepRotX = width;
                sweepPosition -= Time.deltaTime * sweepSpeed;
                if (sweepPosition <= 0) sweep = false;
            }
            spawner.rotation = Quaternion.Euler(sweepRotX, 0, 0);
        }
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
            if(BulletColor != numberOfColors-1)
            {
                BulletColor++;
            }
            else
            {
                BulletColor = 0;
            }
            return;
        }
        BulletColor = Random.Range(0, Mathf.RoundToInt(numberOfColors));
    }
    private void FreezeBullets()
    {
        if (stopAfterSeconds == 0) return;
        if (bulletsLifetime > stopAfterSeconds)
        {
            for(int i = 0; i < bulletsCreated.Count; i++)
            {
                bulletsCreated[i].GetComponent<Bullet>().speed = 0;
                freeze = true;
                if (!connectToSpawnerOnStop) return;
                if (bulletsCreated[i] == null) return;
                bulletsCreated[i].transform.SetParent(this.transform.GetChild(0));
            }
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
    private GameObject GetBulletFromPool()
    {
        if (bulletsCreated == null)
        {
            return null;
        }
        for (int i = 0; i < bulletsCreated.Count - 1; i++)
        {
            if (bulletsCreated[i].gameObject == null)
            {
                bulletsCreated.Remove(bulletsCreated[i]);
                continue;
            }
            else if (!bulletsCreated[i].activeSelf)
            {
                return bulletsCreated[i];
            }
        }
        return null;

    }
    private bool AllBulletsOffscreen()
    {
        for(int i = 0; i < bulletsCreated.Count; i++)
        {
            if (bulletsCreated[i].activeSelf)
            {
                return false;
            }
        }
        return true;
    }
    private void DestroyAll()
    {
        for(int i = 0; i < bulletsCreated.Count; i++)
        {
            bulletsCreated.Remove(bulletsCreated[i]);
        }
        Destroy(gameObject);
    }
}
