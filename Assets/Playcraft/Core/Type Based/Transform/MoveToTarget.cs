using UnityEngine;

namespace Playcraft
{
    public class MoveToTarget : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float speed;
        [SerializeField] float smoothTime = .1f;
        //[SerializeField] float stoppingDistance;
        
        public void SetTarget(Transform value) { target = value; }
        public void SetSpeed(float value) { speed = value; }
        
        Vector3 position => transform.position;
        //float targetDistance => Vector3.Distance(target.position, position);
        //bool atTarget => targetDistance <= stoppingDistance;
        Vector3 movement => newPosition - transform.position;
        
        Vector3 velocity;
        //bool wasAtTarget;
        Vector3 newPosition;
        
        void Update()
        {
            if (!target) return;
            newPosition = Vector3.SmoothDamp(position, target.position, ref velocity, smoothTime, speed);
            //CheckForTarget();
            //wasAtTarget = atTarget;
            transform.Translate(movement);
        }
        
        /*void CheckForTarget()
        {
            if (!atTarget || wasAtTarget) return;
            var response = target.GetComponent<GameObjectRelay>();
            if (response) response.SetObject(gameObject);
        }*/
    }    
}
