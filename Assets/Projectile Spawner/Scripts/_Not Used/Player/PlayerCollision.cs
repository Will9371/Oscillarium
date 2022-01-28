using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] HealthManager healthManager;
    [SerializeField] Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            if (other.GetComponent<ColorChange>().colorIndex == player.colorChange.colorIndex)
            {
                other.gameObject.SetActive(false);
                player.ammo.AddAmmo(other.GetComponent<ColorChange>().colorIndex);
            }
        }
    }

    public void HitInTheHitBox(GameObject other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            healthManager.Damage(1);
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Enemy"))
        {
            healthManager.Damage(1);
        }
    }
}
