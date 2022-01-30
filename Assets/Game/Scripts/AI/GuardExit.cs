using UnityEngine;
using Playcraft.PredictiveMovement;

public class GuardExit : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GoalieAIMono movement;
    
    Vector3 direction;
    
    void OnEnable()
    {
        direction = transform.position.normalized;
        movement.enabled = false;
    }

    void Update() { transform.Translate(speed * Time.deltaTime * direction); }
    
    void OnBecameInvisible() 
    { 
        if (!enabled) return;
        gameObject.SetActive(false); }
}
