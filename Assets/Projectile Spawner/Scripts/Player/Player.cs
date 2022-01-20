using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerShooting shooting = null;
    public BulletSpawner bulletSpawner = null;
    public ColorChange colorChange = null;
    public PlayerCollision playerCollision = null;
    public HealthManager healthManager = null;
    public Respawn respawn = null;
    public Ammo ammo = null;
}
