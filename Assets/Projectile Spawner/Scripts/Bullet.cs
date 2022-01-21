using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Renderer rend;
    
    [NonSerialized] public BulletData data;
    [NonSerialized] public Faction faction;
    
    private float bulletLifetime => Time.time - startTime;
    private float startTime;
    private Vector3 center;
    private Vector3 offset;
    private float yPhase;

    public void Initialize(BulletData data, Faction faction)
    {
        this.data = data;
        this.faction = faction;
        
        center = transform.position;
        startTime = Time.time;
        
        if (data.reverseAfterSeconds > 0)
            Invoke(nameof(Reverse), data.reverseAfterSeconds);        
    }
    
    private void Update()
    {
        Step();
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
            offset.y = data.yAmplitude * data.yCurve.Evaluate(yPhase);
        
        transform.position = center + offset;
    }

    private void BulletPositionDestroyer()
    {
        if (data.dontDestroyOffscreen) return;
        if (!rend.isVisible && bulletLifetime > data.maxLifetime)
            gameObject.SetActive(false);
    }

    private void Reverse()
    {
        data.speed *= -1;
    }

    private void BulletTimeDestroyer()
    {
        if (data.destroyAfterSeconds == 0) 
            return;
        
        if (data.destroyAfterSeconds <= bulletLifetime)
            gameObject.SetActive(false);
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
