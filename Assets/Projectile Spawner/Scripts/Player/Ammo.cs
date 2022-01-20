using UnityEngine.UI;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private Player player = null;
    public float whiteMax = 1000;
    public float whiteAmmo = 100;
    public float purpleMax = 1000;
    public float purpleAmmo = 100;
    private float addAmountOnAbsorb = 25;
    private readonly float ammoPerFrame = 2f;
    [SerializeField] private Image whiteAmmoBar = null;
    [SerializeField] private Image purpleAmmoBar = null;
    private AudioManager audioManager = null;
    private void Start()
    {
        player = this.GetComponent<Player>();
        audioManager = FindObjectOfType<AudioManager>();
        UpdateAmmoBarSize();
    }
    private void Update()
    {
        ShootingSounds();
    }
    public void ReduceAmmo()
    {
        if (player.shooting.bulletColor == 0 && whiteAmmo > 0)
        {
            player.bulletSpawner.startFiring = true;
            whiteAmmo -= ammoPerFrame;
        }
        else if (player.shooting.bulletColor == 1 && purpleAmmo > 0)
        {
            player.bulletSpawner.startFiring = true;
            purpleAmmo -= ammoPerFrame;
        }
        else
        {
            player.bulletSpawner.startFiring = false;
        }
        UpdateAmmoBarSize();
    }
    public void AddAmmo(int color)
    {
        if (color == 0 && whiteAmmo < whiteMax) whiteAmmo+= addAmountOnAbsorb;
        if (color == 1 && purpleAmmo < whiteMax) purpleAmmo += addAmountOnAbsorb;
        UpdateAmmoBarSize();
        audioManager.PickUp();
    }
    private void UpdateAmmoBarSize()
    {
        whiteAmmoBar.rectTransform.sizeDelta = new Vector2(495 * (whiteAmmo / whiteMax), 15);
        purpleAmmoBar.rectTransform.sizeDelta = new Vector2(495 * (purpleAmmo / purpleMax), 15);
    }
    private void ShootingSounds()
    {
        if (player.bulletSpawner.startFiring == true)
        {
            audioManager.PlayShoot();
            return;
        }
        audioManager.StopShoot();
    }


}
