using UnityEngine;

public class GetOffset : MonoBehaviour
{
    [SerializeField] BulletData data;
    [SerializeField] Vector3Event Offset;

    Vector3 offset;
    float yPhase;
    float xPhase;
    
    void OnEnable()
    {
        xPhase = 0f;
        yPhase = 0f;
        offset = Vector3.zero;
        Update();
    }

    // Update is called once per frame
    void Update()
    {
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
        
        Offset.Invoke(offset);  
    }
}
