using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Renderer rend;

    public float speed = 5;
    public float yAmplitude = 0;
    public float yFrequency = 0;
    public bool reverseSine = false;
    public float reverseAfterSeconds = 0;
    public float maxLifetime = 3f;
    public BulletSpawner bulletSpawner;
    public Faction faction;
    public AnimationCurve yCurve;
    
    private float bulletLifetime => Time.time - startTime;
    private float startTime;
    private bool reversed = false;
    private bool dontDestroyOffscreen;
    private Vector3 center;
    private Vector3 curveOffset;
    private Vector3 offset => reverseSine ? curveOffset * -1f : curveOffset;
    private float yPhase;

    private void OnEnable()
    {
        if (bulletSpawner == null) return;
        dontDestroyOffscreen = bulletSpawner.doNotDestroyOffScreen;
        center = transform.position;
        startTime = Time.time;
    }
    
    private void Update()
    {
        Step();
        Reverse();
        BulletTimeDestroyer();
        BulletPositionDestroyer();
    }
    
    private void Step()
    {
        center += speed * Time.deltaTime * transform.right;  
        
        yPhase += Time.deltaTime * yFrequency;
        while (yPhase > 1f)
            yPhase -= 1f;
        
        if (yAmplitude != 0f)
            curveOffset.y = yAmplitude * yCurve.Evaluate(yPhase);
        
        transform.position = center + offset;
    }

    private void BulletPositionDestroyer()
    {
        if (dontDestroyOffscreen) return;
        if (!rend.isVisible && bulletLifetime > maxLifetime)
            gameObject.SetActive(false);
    }

    private void Reverse()
    {
        if (reverseAfterSeconds == 0 || bulletLifetime <= reverseAfterSeconds || reversed) 
            return;

        speed *= -1;
        reversed = true;
    }

    private void BulletTimeDestroyer()
    {
        if (!bulletSpawner || bulletSpawner.destroyAfterSeconds == 0) 
            return;
        
        if(bulletSpawner.destroyAfterSeconds <= bulletLifetime)
            gameObject.SetActive(false);
    }
    
    void OnDisable()
    {
        reversed = false;
    }
}

/*private void SineWave()
{
    //bulletLifetime += Time.deltaTime;
    center += speed * Time.deltaTime * transform.right;
    
    if (sineAmplitude != 0f)
        sineOffset.y = sineAmplitude * Mathf.Sin(bulletLifetime * sineFrequency);
        
    transform.position = center + offset;
}*/
