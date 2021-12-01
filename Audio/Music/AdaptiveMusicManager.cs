using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicManager : MonoBehaviour
{
    public AdaptiveMusicLayer highHealthLayer;
    public LocationBasedAdaptiveMusicLayer begibbonLayer;

    List<AdaptiveMusicLayer> layers;

    const float enemyDistanceCheckTime = 0.33f;
    float enemyDistanceCheckTimer;

    void Awake()
    {
        layers = new List<AdaptiveMusicLayer>();
        layers.Add(highHealthLayer);
        layers.Add(begibbonLayer);
    }

    public void StartAdaptiveMusicLayers()
    {
        highHealthLayer.audioSource.Play();
        begibbonLayer.audioSource.Play();
    }

    void Update()
    {
        enemyDistanceCheckTimer -= Time.deltaTime;
        if (enemyDistanceCheckTimer <= 0)
        {
            enemyDistanceCheckTimer = enemyDistanceCheckTime;
            EnemyDistanceCheck();
        }
    }

    public void EnemyDistanceCheck()
    {
        foreach (Enemy enemy in Overseer.Instance.enemyManager.instantiatedEnemies)
        {
            if (enemy is Begibbon)
            {
                AssignEnemyLayerLocation(begibbonLayer, enemy);
            }
        }
    }

    void AssignEnemyLayerLocation(LocationBasedAdaptiveMusicLayer layer, Enemy enemy)
    {
        if (Vector2.Distance(Camera.main.transform.position, enemy.transform.position) < layer.locationDistToCamera)
        {
            layer.location = enemy.transform;
        }
    }

    public void SetLevels()
    {
        foreach (AdaptiveMusicLayer layer in layers)
        {
            layer.SetLevels();
        }
    }
}
