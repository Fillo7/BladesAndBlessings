using System.Collections.Generic;
using UnityEngine;

public class CaveAbilities : MonoBehaviour
{
    [SerializeField] private List<Transform> hatchlingSpawnPoints;
    [SerializeField] private GameObject hatchling;

    public void SpawnHatchlings()
    {
        int spawnIndex = Random.Range(0, hatchlingSpawnPoints.Count);
        Instantiate(hatchling, hatchlingSpawnPoints[spawnIndex]);

        spawnIndex = Random.Range(0, hatchlingSpawnPoints.Count);
        Instantiate(hatchling, hatchlingSpawnPoints[spawnIndex]);
    }
}
