using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    private Enemy lastSpawnedEnemy;

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
        lastSpawnedEnemy = lastSpawnedGameObject.GetComponent<Enemy>();
    }

    bool IsLastSpawnedEnemyDead()
    {
        return lastSpawnedEnemy == null || !lastSpawnedEnemy.IsAlive(); // not (yet) existing or dead
    }

    // Just to show the spawner in Scene-View
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
