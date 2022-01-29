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
    float xPhase;
    float speed;

    public void Initialize(BulletData data)
    {
        this.data = data;
        speed = data.speed;
        offset = Vector3.zero;
        
        center = transform.position;
        startTime = Time.time;
        Step();
        
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
        center += speed * Time.deltaTime * transform.right;  
        
        if (data.yAmplitude > 0f)
        {
            yPhase += Time.deltaTime * data.yFrequency;
            while (yPhase > 1f)
                yPhase -= 1f;
        
            offset.y = data.yAmplitude * data.yCurve.Evaluate(yPhase);
        }
        
        if (data.xAmplitude > 0f)
        {    
            xPhase += Time.deltaTime * data.xFrequency;
            while (xPhase > 1f)
                xPhase -= 1f;
                
            offset.x = data.xAmplitude * data.xCurve.Evaluate(xPhase);
        }
        
        transform.position = center + offset;
    }

    void BulletPositionDestroyer()
    {
        if (data.dontDestroyOffscreen) return;
        if (!rend.isVisible && bulletLifetime > data.maxLifetime)
            gameObject.SetActive(false);
    }

    void Reverse() { speed *= -1; }

    void BulletTimeDestroyer()
    {
        if (data.destroyAfterSeconds == 0) 
            return;
        
        if (data.destroyAfterSeconds <= bulletLifetime)
            gameObject.SetActive(false);
    }
}