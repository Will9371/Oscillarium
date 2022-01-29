using UnityEngine;
using Playcraft.Pooling;
using Playcraft.PredictiveMovement;

public class BulletTrail : MonoBehaviour
{
    Lookup lookup => Lookup.instance;
    ObjectPoolMaster pool => ObjectPoolMaster.instance;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] BulletInfo bulletInfo;
    [SerializeField] GoalieAIMono movement;
    [SerializeField] Vector2 spawnRateRange = new Vector2(.5f, 1f);
    [SerializeField] Vector2 changeColorRange = new Vector2(.3f, .6f);
    [SerializeField] bool flyTowardsTarget;
    
    BulletData bulletData => bulletInfo.data[0];
    
    int bulletColorIndex;
    
    void OnEnable()
    {
        Spawn();
        RandomizeColor();
    }
    
    void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
        CancelInvoke(nameof(RandomizeColor));
    }
    
    void Spawn()
    {
        var bulletPosition = transform.position;
        var bulletObject = pool.Spawn(bulletPrefab, bulletPosition);
        var polarity = flyTowardsTarget ? 1f : -1f;
        bulletObject.transform.right = polarity * movement.moveDirection.normalized;

        bulletObject.GetComponent<ColorChange>().SetColor(bulletColorIndex);
        
        var bullet = bulletObject.GetComponent<Bullet>();
        bullet.Initialize(bulletData);
        Invoke(nameof(Spawn), Random.Range(spawnRateRange.x, spawnRateRange.y));
    }
    
    void RandomizeColor()
    {
        bulletColorIndex = Random.Range(0, lookup.colorCount);
        Invoke(nameof(RandomizeColor), Random.Range(changeColorRange.x, changeColorRange.y));
    }
}
