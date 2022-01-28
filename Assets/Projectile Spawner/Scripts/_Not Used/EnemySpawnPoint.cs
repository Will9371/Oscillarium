using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject[] enemies = null;
    [SerializeField] float timeBetweenSpawns = 0;
    [SerializeField] float chanceForSpawn = 0;
    private float timeSinceLastSpawn = 0;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn > timeBetweenSpawns)
        {
            timeSinceLastSpawn = 0;
            float rand = Random.Range(0, 100);
            if(rand <= (chanceForSpawn * 100))
            {
                int randIndex = Random.Range(0, enemies.Length);
                Instantiate(enemies[randIndex], transform.position, transform.rotation);
            }
        }
    }
}
