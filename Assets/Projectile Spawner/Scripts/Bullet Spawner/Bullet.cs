using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Renderer rend;
    
    [NonSerialized] public BulletData data;
    
    float bulletLifetime => Time.time - startTime;
    float startTime;
    Vector3 center;
    Vector3 offset;
    float yPhase;

    public void Initialize(BulletData data)
    {
        this.data = data;
        
        center = transform.position;
        startTime = Time.time;
        
        if (data.reverseAfterSeconds > 0)
            Invoke(nameof(Reverse), data.reverseAfterSeconds);        
    }
    
    void Update()
    {
        Step();
        BulletTimeDestroyer();
        BulletPositionDestroyer();
    }
    
    void Step()
    {
        center += data.speed * Time.deltaTime * transform.right;  
        
        yPhase += Time.deltaTime * data.yFrequency;
        while (yPhase > 1f)
            yPhase -= 1f;
        
        if (data.yAmplitude != 0f)
            offset.y = data.yAmplitude * data.yCurve.Evaluate(yPhase);
        
        transform.position = center + offset;
    }

    void BulletPositionDestroyer()
    {
        if (data.dontDestroyOffscreen) return;
        if (!rend.isVisible && bulletLifetime > data.maxLifetime)
            gameObject.SetActive(false);
    }

    void Reverse()
    {
        data.speed *= -1;
    }

    void BulletTimeDestroyer()
    {
        if (data.destroyAfterSeconds == 0) 
            return;
        
        if (data.destroyAfterSeconds <= bulletLifetime)
            gameObject.SetActive(false);
    }
}