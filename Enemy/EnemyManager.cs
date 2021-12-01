using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public List<Enemy> instantiatedEnemies;

    public Enemy GetEnemy()
    {
        return enemies[Random.Range(0, enemies.Count)];
    }

    public void ResetEnemies()
    {
        foreach (Enemy enemy in instantiatedEnemies)
        {
            enemy.ResetMe();
        }
    }
}
