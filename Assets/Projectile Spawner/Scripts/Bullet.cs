using System;
using UnityEngine;

[Serializable]
public struct BulletData
{
    public Faction faction;
    public float speed;
    public float yAmplitude;
    public float yFrequency;
    public bool reverseSine;
    public float reverseAfterSeconds;
    public float maxLifetime;
    public bool dontDestroyOffscreen;
    public float destroyAfterSeconds;
    
    public BulletData(BulletData template)
    {
        speed = template.speed;
        yAmplitude = template.yAmplitude;
        yFrequency = template.yFrequency;
        reverseSine = template.reverseSine;
        reverseAfterSeconds = template.reverseAfterSeconds;
        maxLifetime = template.maxLifetime;
        dontDestroyOffscreen = template.dontDestroyOffscreen;
        faction = template.faction;
        destroyAfterSeconds = template.destroyAfterSeconds;
    }    
}

public class Bullet : MonoBehaviour
{
    [SerializeField] Renderer rend;
    
    [NonSerialized] public BulletData data;
    public AnimationCurve yCurve;
    
    private float bulletLifetime => Time.time - startTime;
    [SerializeField] [ReadOnly] 
    private float startTime;
    private bool reversed = false;
    private bool dontDestroyOffscreen;  // * Move to struct
    private Vector3 center;
    private Vector3 curveOffset;
    private Vector3 offset => data.reverseSine ? curveOffset * -1f : curveOffset;
    private float yPhase;

    private void OnEnable()
    {
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
        center += data.speed * Time.deltaTime * transform.right;  
        
        yPhase += Time.deltaTime * data.yFrequency;
        while (yPhase > 1f)
            yPhase -= 1f;
        
        if (data.yAmplitude != 0f)
            curveOffset.y = data.yAmplitude * yCurve.Evaluate(yPhase);
        
        transform.position = center + offset;
    }

    private void BulletPositionDestroyer()
    {
        if (dontDestroyOffscreen) return;
        if (!rend.isVisible && bulletLifetime > data.maxLifetime)
            gameObject.SetActive(false);
    }

    private void Reverse()
    {
        if (data.reverseAfterSeconds == 0 || bulletLifetime <= data.reverseAfterSeconds || reversed) 
            return;

        data.speed *= -1;
        reversed = true;
    }

    private void BulletTimeDestroyer()
    {
        if (data.destroyAfterSeconds == 0) 
            return;
        
        if (data.destroyAfterSeconds <= bulletLifetime)
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
