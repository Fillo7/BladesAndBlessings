using UnityEngine;

public class EnemyInstance : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay;

    private GameObject enemy;
    private bool enemySpawned = false;

    public void SpawnEnemy()
    {
        Invoke("SpawnEnemyPrivate", spawnDelay);
    }

    public GameObject GetEnemyPrefab()
    {
        return enemyPrefab;
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public float GetSpawnDelay()
    {
        return spawnDelay;
    }

    public GameObject GetEnemy()
    {
        return enemy;
    }

    public bool IsEnemySpawned()
    {
        return enemySpawned;
    }

    private void SpawnEnemyPrivate()
    {
        enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemySpawned = true;
    }
}
