using System.Collections.Generic;
using UnityEngine;


public class AvoidAI : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.2f;
    [SerializeField] float radius = 2f;
    [SerializeField] float avoidStrength = 1f;
    [SerializeField] float avoidMax = 1f;

    List<Transform> nearbyAI = new List<Transform>();
    public Vector3 avoidVector;

    void OnEnable()
    {
        InvokeRepeating(nameof(Refresh), refreshRate, refreshRate);
    }
    
    void OnDisable()
    {
        CancelInvoke(nameof(Refresh));
    }
    
    void Update()
    {
        transform.Translate(Time.deltaTime * avoidVector);
    }
    
    void Refresh()
    {
        FindNearbyAI();
        SetAvoidance();
    }
    
    void FindNearbyAI()
    {
        nearbyAI.Clear();
    
        var nearby = Physics2D.OverlapCircleAll(transform.position, radius);
        
        foreach (var other in nearby)
            if (other.GetComponent<AvoidAI>())
                nearbyAI.Add(other.transform);
    }
    
    void SetAvoidance()
    {
        avoidVector = Vector3.zero;
        foreach (var ai in nearbyAI)
        {
            var direction = (transform.position - ai.position).normalized;
            var distance = Vector3.Distance(transform.position, ai.position);
            avoidVector += direction/(distance + 1) * avoidStrength;
        }
        
        avoidVector = Vector3.ClampMagnitude(avoidVector, avoidMax);
    }
    
    //void OnDrawGizmos() { Gizmos.DrawWireSphere(transform.position, radius); }
}
