using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5;
    public float sineAmplitude = 0;
    public float sineFrequency = 0;
    public float yDirection = 1f;
    public bool reverseSine = false;
    public float reverseAfterSeconds = 0;
    public BulletSpawner bulletSpawner;
    public bool isEnemy;

    //Private
    private float sineWavePosition = 0;
    private float sineTime = 0;
    private float bulletLifetime = 0;
    private bool reversed = false;
    private bool dontDestroyOffscreen;

    private void Start()
    {
        if (reverseSine)
        {
            sineTime = sineFrequency;
        }
    }
    private void OnEnable()
    {
        if (reverseSine)
        {
            sineTime = sineFrequency;
        }
        if (bulletSpawner == null) return;
        dontDestroyOffscreen = bulletSpawner.doNotDestroyOffScreen;
    }
    private void Update()
    {
        bulletLifetime += Time.deltaTime;
        transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        SineWave();
        Reverse();
        BulletTimeDestroyer();
        BulletPositionDestroyer();
    }
    private void BulletPositionDestroyer()
    {
        if (dontDestroyOffscreen) return;
        if (!this.GetComponent<Renderer>().isVisible && bulletLifetime > 3)
        //if(this.transform.position.y >= 8 || this.transform.position.y <= -8 || this.transform.position.z >= 30 || this.transform.position.z <= -30)
        {
            PoolBullet();
        }
    }
    private void SineWave()
    {
        if (sineAmplitude == 0) return;
        sineTime += yDirection;
        sineWavePosition = Mathf.Lerp(-sineAmplitude, sineAmplitude, sineTime / sineFrequency);
        transform.Translate(Vector3.up * sineWavePosition);
        if (sineTime >= sineFrequency) yDirection = -1;
        if (sineTime <= 0) yDirection = 1;
    }
    private void Reverse()
    {
        if (reverseAfterSeconds == 0) return;
        if (bulletLifetime > reverseAfterSeconds && !reversed)
        {
            speed *= -1;
            reversed = true;
        }
    }
    public void PoolBullet()
    {
        reversed = false;
        sineWavePosition = 0;
        sineTime = 0;
        bulletLifetime = 0;
        this.gameObject.SetActive(false);
    }
    private void BulletTimeDestroyer()
    {
        if (bulletSpawner == null) return;
        if (bulletSpawner.destroyAfterSeconds == 0) return;
        if(bulletSpawner.destroyAfterSeconds <= bulletLifetime)
        {
            PoolBullet();
        }
    }
}
