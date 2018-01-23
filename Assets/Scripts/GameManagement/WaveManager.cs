using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<EnemyWave> waves = new List<EnemyWave>();

    bool firstWaveSpawned = false;
    private int currentWaveIndex = -1;

    public void SpawnNextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            return;
        }

        waves[currentWaveIndex].SpawnWave();
        firstWaveSpawned = true;
    }

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex + 1;
    }

    public int GetWaveCount()
    {
        return waves.Count;
    }

    public int GetCurrentWaveHealth()
    {
        return waves[currentWaveIndex].GetCurrentWaveHealth();
    }

    public int GetTotalWaveHealth()
    {
        return waves[currentWaveIndex].GetTotalWaveHealth();
    }

    public bool IsFirstWaveSpawned()
    {
        return firstWaveSpawned;
    }

    public bool IsCurrentWaveDefeated()
    {
        return waves[currentWaveIndex].IsWaveDefeated();
    }

    public bool AreAllWavesDefeated()
    {
        return (currentWaveIndex + 1) == waves.Count && waves[currentWaveIndex].IsWaveDefeated();
    }
}
