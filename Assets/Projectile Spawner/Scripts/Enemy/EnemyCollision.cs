using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            this.GetComponent<HealthManager>().Damage(2);
            if(other.GetComponent<ColorChange>().color != this.GetComponent<ColorChange>().color) this.GetComponent<HealthManager>().Damage(8);
            other.gameObject.SetActive(false);
        }
    }
}
