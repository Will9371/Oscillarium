using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Renderer rend;
    
    [NonSerialized] public BulletData data;
    [NonSerialized] public float speed;
    
    float bulletLifetime => Time.time - startTime;
    float startTime;
    Vector3 center;
    Vector3 offset;
    float yPhase;
    float xPhase;
    

    public void Initialize(BulletData data)
    {
        startTime = Time.time;
        this.data = data;
        speed = data.speed;
        center = transform.position;
        offset = Vector3.zero;
        xPhase = 0f;
        yPhase = 0f;
        
        Step();
        
        if (data.reverseAfterSeconds > 0)
            Invoke(nameof(Reverse), data.reverseAfterSeconds);        
    }
    
    void Update()
    {
        Step();
        
        // REFACTOR: use Invoke instead of Update
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
        
        // HACK:
        /*if (speed > 0f)
            transform.position = center + offset;
        else
            transform.localPosition = center + offset;*/
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
    
    //public void SetCenterToLocal() { center = transform.localPosition - offset; }
}