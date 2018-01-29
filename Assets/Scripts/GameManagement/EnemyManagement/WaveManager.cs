using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<EnemyWave> waves = new List<EnemyWave>();
    [SerializeField] private Slider waveSlider;
    [SerializeField] private Text waveText;

    bool firstWaveSpawned = false;
    private int currentWaveIndex = -1;

    public void Update()
    {
        waveSlider.value = GetCurrentWaveHealth();
    }

    public void SpawnNextWave()
    {
        SpawnNextWave(0.0f);
    }

    public void SpawnNextWave(float delay)
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            return;
        }

        waves[currentWaveIndex].SpawnWave(delay);
        waveSlider.maxValue = GetTotalWaveHealth();
        waveText.text = "Wave " + (currentWaveIndex + 1) + " / " + waves.Count;
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
