using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxCollision : MonoBehaviour
{
    private Player player = null;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.playerCollision.HitInTheHitBox(other.gameObject);
    }
}
