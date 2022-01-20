using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    Player player = null;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyProjectile")
        {
            if (other.GetComponent<ColorChange>().color == player.colorChange.color)
            {
                other.gameObject.SetActive(false);
                player.ammo.AddAmmo(other.GetComponent<ColorChange>().color);
                return;
            }
        }
    }

    public void HitInTheHitBox(GameObject other)
    {
        if(other.tag == "EnemyProjectile")
        {
            this.GetComponent<HealthManager>().Damage(1);
            other.gameObject.SetActive(false);
        }
        if(other.tag == "Enemy")
        {
            this.GetComponent<HealthManager>().Damage(1);
        }
    }
}
