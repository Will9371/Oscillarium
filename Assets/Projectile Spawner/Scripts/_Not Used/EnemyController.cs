using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool lookAtPlayer = true;
    private Player player = null;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        transform.position = new Vector3(0, 0, -15f);
    }
    private void Update()
    {
        LookAtPlayer();
    }
    private void LookAtPlayer()
    {
        if (lookAtPlayer)
        {
            transform.LookAt(player.transform);
        }
    }
}
