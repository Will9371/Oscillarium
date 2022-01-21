using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    ColorChange color;
    HealthManager health;
    
    void Start()
    {
        color = GetComponent<ColorChange>();
        health = GetComponent<HealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            health.Damage(2);
            if (other.GetComponent<ColorChange>().colorIndex != color.colorIndex) health.Damage(8);
            other.gameObject.SetActive(false);
        }
    }
}
