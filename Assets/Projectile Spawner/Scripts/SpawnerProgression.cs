using System;
using System.Collections;
using UnityEngine;

public class SpawnerProgression : MonoBehaviour
{
    [SerializeField] ColorFlash flash;
    [SerializeField] BulletSpawner spawner;
    [SerializeField] SpawnerStage[] stages;
    
    int stageIndex;
    
    void Start()
    {
        BeginStage();
    }

    void BeginStage()
    {
        if (stageIndex >= stages.Length) return;
        var stage = stages[stageIndex];
        spawner.transform.position = stage.position;
        spawner.transform.eulerAngles = stage.rotation;
        spawner.info = stage.spawnInfo;
        spawner.enabled = false;
        
        StartCoroutine(FlashAndChange());
        
        stageIndex++;
        Invoke(nameof(BeginStage), stage.duration);
    }
    
    IEnumerator FlashAndChange()
    {
        yield return StartCoroutine(flash.Process());
        spawner.enabled = true;
    }
}

[Serializable]
public struct SpawnerStage
{
    public float duration;
    public Vector3 position;
    public Vector3 rotation;
    public BulletSpawnerInfo spawnInfo;
}
