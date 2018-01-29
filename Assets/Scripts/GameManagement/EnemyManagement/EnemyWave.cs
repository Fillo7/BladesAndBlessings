using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    [SerializeField] private List<EnemyInstance> enemies = new List<EnemyInstance>();

    private bool waveSpawned = false;

    public void SpawnWave()
    {
        SpawnWave(0.0f);
    }

    public void SpawnWave(float delay)
    {
        Invoke("SpawnWavePrivate", delay);
        waveSpawned = true;
    }

    public bool IsWaveSpawned()
    {
        return waveSpawned;
    }

    public int GetCurrentWaveHealth()
    {
        int waveHealth = 0;

        foreach (EnemyInstance instance in enemies)
        {
            if (!instance.IsEnemySpawned())
            {
                GameObject enemy = instance.GetEnemyPrefab();

                if (enemy != null)
                {
                    waveHealth += enemy.GetComponent<EnemyHealth>().GetBaseHealth();
                }
            }
            else
            {
                GameObject enemy = instance.GetEnemy();

                if (enemy != null)
                {
                    waveHealth += enemy.GetComponent<EnemyHealth>().GetCurrentHealth();
                }
            }
        }

        return waveHealth;
    }

    public int GetTotalWaveHealth()
    {
        int waveHealth = 0;

        foreach (EnemyInstance instance in enemies)
        {
            GameObject enemy = instance.GetEnemyPrefab();

            if (enemy != null)
            {
                waveHealth += enemy.GetComponent<EnemyHealth>().GetBaseHealth();
            }
        }

        return waveHealth;
    }

    public bool IsWaveDefeated()
    {
        bool result = true;

        foreach (EnemyInstance instance in enemies)
        {
            if (instance.GetEnemy() != null || !instance.IsEnemySpawned())
            {
                result = false;
                break;
            }
        }

        return result && waveSpawned;
    }

    private void SpawnWavePrivate()
    {
        foreach (EnemyInstance instance in enemies)
        {
            instance.SpawnEnemy();
        }
    }
}
