using UnityEngine.UI;
using UnityEngine;

// REFACTOR
public class HealthManager : MonoBehaviour
{
    private Player player = null;
    [SerializeField] private GameObject bullets = null;
    public float health = 1;
    [SerializeField] private float deathBullets = 5;
    public bool killPlayer = false;
    [SerializeField] Image bossHealthBar = null;
    private AudioManager audioManager = null;
    [SerializeField] private GameObject winScreen = null;
    
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = GetComponent<Player>();
    }

    //Properties
    public float Health { get => health; private set => health = value; }
    
    public void Damage(float amount)
    {
        if (player != null && player.respawn.isInvulnerable) return;
        if (amount == 2) audioManager.Hit1x();
        if (amount > 2) audioManager.Hit4x();
        health -= amount;
        Death();
        UpdateBossHealth();
    }
    
    private void Death()
    {
        if (health > 0) return;
        for(int i = 0; i < deathBullets; i++)
        {
            float rand = Random.Range(-0.5f, 0.5f);
            GameObject bullet = Instantiate(bullets, transform.position + new Vector3(0, rand, rand), Quaternion.Euler (rand*180,0,0));
            //bullet.GetComponent<Bullet>().speed = 2;
            bullet.GetComponent<ColorChange>().colorIndex = GetComponent<ColorChange>().colorIndex;
        }
        if(gameObject.tag == "Enemy")
        {
            DestroyAndPool();
            return;
        }
        killPlayer = true;
    }
    
    private void DestroyAndPool()
    {
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Bullet>() != null)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
        
        Destroy(gameObject);
    }
    
    private void UpdateBossHealth()
    {
        if (bossHealthBar == null) return;
        bossHealthBar.rectTransform.sizeDelta = new Vector2(1788 * (health / 20000), 45);
        if (health <= 0)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
