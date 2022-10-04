using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Transform spawnPosition;

    [Space]

    [Header("Properties")]
    [Min(1)]
    public int rotationSpeed = 10;
    
    // internal
    private Enemy _lastSpawnedEnemy;

    void Update()
    {
        if (IsLastSpawnedEnemyDead())
        {
            SpawnNewEnemy();
        }

        Spin();
    }

    private void Spin()
    {
        transform.rotation = Quaternion.Euler(0, Time.time * rotationSpeed, 0);
    }

    private void SpawnNewEnemy()
    {
        var positionOfEnemySpawner = spawnPosition.position;
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
