using System;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerProgression : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] Settings settings;
    [SerializeField] References[] references;
    [SerializeField] Stage[] stages;
    [SerializeField] int startAtStage;
    [SerializeField] FloatEvent outputStartTime;
    
    Stage stage => stages[stageIndex];
    float stageFlashTime => stage.spawners.Length > 0 ? settings.totalFlashTime : 0f;
    
    int stageIndex;
    float nextStageTime;
    bool inTransition;

    void Start()
    {
        SetStartStage();
        SetStartTime();
        BeginTransition();
    }
    
    void Update()
    {
        if (music.time >= nextStageTime && !inTransition)
            BeginTransition();
    }
    
    void SetStartStage()
    {
        if (startAtStage >= stages.Length)
            startAtStage = stages.Length - 1;
    
        stageIndex = startAtStage;        
    }
    
    void SetStartTime()
    {
        var startTime = 0f;
        for (int i = 0; i < startAtStage; i++)
        {
            startTime += stages[i].duration;
            //startTime += stageFlashTime;
        }
        outputStartTime.Invoke(startTime);
        nextStageTime = startTime;     
    }

    void BeginTransition()
    {
        if (stageIndex >= stages.Length) return;
        inTransition = true;
        
        foreach (var reference in references)
            reference.spawner.enabled = false;
        
        for (int i = 0; i < stage.spawners.Length; i++)
        {
            var spawnProcess = references[i].spawner;
            var spawnData = stage.spawners[i];
            spawnProcess.transform.position = spawnData.position;
            spawnProcess.transform.eulerAngles = spawnData.rotation;
            spawnProcess.info = spawnData.info;
            StartCoroutine(references[i].flash.Process());
        }

        var flashTime = stageFlashTime;
        Invoke(nameof(BeginStage), flashTime);
        nextStageTime += stage.duration;
        //Invoke(nameof(BeginTransition), stage.duration);
    }
    
    void BeginStage()
    {
        //Debug.Log($"Beginning stage {stageIndex} at time {Time.time}");
        for (int i = 0; i < stage.spawners.Length; i++)
            references[i].spawner.enabled = true;

        stage.onBegin.Invoke();
        stageIndex++;
        inTransition = false;
    }
    
    void OnValidate()
    {
        foreach (var reference in references)
        {
            reference.flash.rend = reference.cue;
            reference.flash.flashTime = settings.flashTime;
            reference.flash.flashCount = settings.flashCount;
            reference.flash.defaultColor = settings.defaultColor;
        }
    }
    
    [Serializable]
    public struct Settings
    {
        public float flashTime;
        public int flashCount;
        public Color defaultColor;
        public float totalFlashTime => flashTime * flashCount * 2;
    }
    
    [Serializable]
    public struct References
    {
        public BulletSpawner spawner;
        [HideInInspector] 
        public ColorFlash flash;
        public SpriteRenderer cue;
    }
    
    [Serializable]
    public struct Stage
    {
        public float duration;
        public Spawner[] spawners;
        public UnityEvent onBegin;
    }
    
    [Serializable]
    public struct Spawner
    {
        public Vector3 position;
        public Vector3 rotation;
        public BulletSpawnerInfo info;
    }
}


