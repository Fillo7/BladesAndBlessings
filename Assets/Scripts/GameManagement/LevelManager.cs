using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameManager gameManager;
    private WaveManager waveManager;
    private PlayerHealth playerHealth;

    private float waveSpawnDelay = 3.0f;
    private bool gameOver = false;
    private bool victory = false;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
        waveManager = GetComponent<WaveManager>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (gameOver || victory)
        {
            return;
        }

        if (playerHealth.IsDead())
        {
            gameOver = true;
            Invoke("TriggerGameOver", 5.0f);
        }

        if (!waveManager.IsFirstWaveSpawned())
        {
            waveManager.SpawnNextWave();
        }

        if (waveManager.IsCurrentWaveDefeated())
        {
            if (waveManager.AreAllWavesDefeated())
            {
                victory = true;
                DespawnEnemies();
                Invoke("TriggerVictory", 3.0f);
            }
            else
            {
                waveManager.SpawnNextWave(waveSpawnDelay);
            }
        }
    }

    public bool IsGameActive()
    {
        return !gameOver && !victory;
    }

    public void TriggerGameOver()
    {
        gameManager.TriggerGameOver();
    }

    public void TriggerVictory()
    {
        gameManager.TriggerVictory();
    }

    private void DespawnEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null && !enemies[i].GetComponent<EnemyHealth>().IsDead())
            {
                Destroy(enemies[i]);
            }
        }
    }
}
