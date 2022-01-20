using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Player player = null;


    public int bulletColor;
    private void Start()
    {
        player = this.GetComponent <Player>();
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
        player.bulletSpawner.BulletColor = player.colorChange.color;
        bulletColor = player.colorChange.color;
    }
    private void TurnLeft()
    {
        this.transform.rotation = Quaternion.Euler(0, 180, 0);
        player.bulletSpawner.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.bulletSpawner.transform.localPosition = new Vector3(0, 0, -0.568000019f);
    }
    private void TurnRight()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.bulletSpawner.transform.rotation = Quaternion.Euler(180, 0, 0);
        player.bulletSpawner.transform.localPosition = new Vector3(0, 0, -0.568000019f);
    }
    
}
