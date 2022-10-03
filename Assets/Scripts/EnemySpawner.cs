using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    private Enemy _lastSpawnedEnemy;

    void Update()
    {
        if (IsLastSpawnedEnemyDead())
        {
            SpawnNewEnemy();
        }
    }

    private void SpawnNewEnemy()
    {
        var positionOfEnemySpawner = transform.position;
        GameObject lastSpawnedGameObject = Instantiate(enemyPrefab, positionOfEnemySpawner, Quaternion.identity);
        _lastSpawnedEnemy = lastSpawnedGameObject.GetComponent<Enemy>(); // remember reference so we don't spawn more than we want
    }

    bool IsLastSpawnedEnemyDead()
    {
        return _lastSpawnedEnemy == null || !_lastSpawnedEnemy.IsAlive(); // not (yet) existing or dead
    }

    // Just to show the spawner in Scene-View
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
