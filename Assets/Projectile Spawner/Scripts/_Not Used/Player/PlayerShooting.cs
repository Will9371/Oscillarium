using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Player player = null;

    public int bulletColor;
    
    private void Start()
    {
        player = GetComponent<Player>();
    }
    
    public void ShootLeft()
    {
        TurnLeft();
        ChangeBulletColor();
        player.ammo.ReduceAmmo();
    }
    
    public void ShootRight()
    {
        TurnRight();
        ChangeBulletColor();
        player.ammo.ReduceAmmo();
    }
    
    private void ChangeBulletColor()
    {
        player.bulletSpawner.bulletColorIndex = player.colorChange.colorIndex;
        bulletColor = player.colorChange.colorIndex;
    }
    
    private void TurnLeft()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        player.bulletSpawner.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.bulletSpawner.transform.localPosition = new Vector3(0, 0, -0.568000019f);
    }
    
    private void TurnRight()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        player.bulletSpawner.transform.rotation = Quaternion.Euler(180, 0, 0);
        player.bulletSpawner.transform.localPosition = new Vector3(0, 0, -0.568000019f);
    }
}
