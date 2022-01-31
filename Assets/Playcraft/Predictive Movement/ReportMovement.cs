using UnityEngine;

public class ReportMovement : MonoBehaviour
{
    [SerializeField] Vector3Event output;
    
    Vector3 lastPosition;
    
    void OnEnable() { lastPosition = transform.position; }

    void Update()
    {
        output.Invoke( transform.position - lastPosition);
        lastPosition = transform.position;
    }
}
