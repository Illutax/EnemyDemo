using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MultipleEnemySpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject enemyPrefab;
    public Transform spawnPosition;
    
    [FormerlySerializedAs("rotationSpeed")]
    [Header("Spawning")]
    [Min(1)]
    public int spawnerRotationSpeed = 50;
    public int numberOfEnemiesToSpawn = 10;
    public float spawnDelay = 0.3f;
    
    // internal
    private List<Enemy> _lastSpawnedEnemies = new();
    private float _nextSpawnTime;

    void Update()
    {
        if (TimeForAnotherEnemy() && CanAffordAnotherEnemy())
        {
            _nextSpawnTime = Time.time + spawnDelay;
            SpawnNewEnemy();
        }

        Spin();
    }

    private bool TimeForAnotherEnemy() => Time.time > _nextSpawnTime;

    private void Spin() => transform.rotation = Quaternion.Euler(0, Time.time * spawnerRotationSpeed, 0);

    private void SpawnNewEnemy()
    {
        var positionOfEnemySpawner = spawnPosition.position;
        GameObject lastSpawnedGameObject = Instantiate(enemyPrefab, positionOfEnemySpawner, Quaternion.identity);
        _lastSpawnedEnemies.Add(lastSpawnedGameObject.GetComponent<Enemy>()); // remember reference so we don't spawn more than we want
    }

    private bool CanAffordAnotherEnemy()
    {
        _lastSpawnedEnemies = _lastSpawnedEnemies.Select(it => it.IsAlive() ? it : null).Where(it => it != null).ToList();
        return _lastSpawnedEnemies.Count < numberOfEnemiesToSpawn;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPosition.position, 1);
    }
}
