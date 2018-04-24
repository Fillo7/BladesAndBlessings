using System.Collections.Generic;
using UnityEngine;

public class CaveAbilities : MonoBehaviour
{
    [SerializeField] private List<Transform> hatchlingSpawnPoints;
    [SerializeField] private GameObject hatchling;
    [SerializeField] private GameObject rock;

    private float[] area1Boundaries = {-12.0f, -5.0f, -19.0f, 0.0f};
    private float[] area2Boundaries = {5.0f, 12.0f, -19.0f, 0.0f};
    private float[] area3Boundaries = {-8.0f, 8.0f, -24.0f, -15.0f};

    public void SpawnHatchlings()
    {
        for (int i = 0; i < 2; i++)
        {
            int spawnIndex = Random.Range(0, hatchlingSpawnPoints.Count);
            Instantiate(hatchling, hatchlingSpawnPoints[spawnIndex]);
        }
    }

    public void SpawnRocks()
    {
        for (int i = 0; i < 25; i++)
        {
            int area = Random.Range(0, 3);
            float xPosition = 0.0f;
            float zPosition = 0.0f;

            if (area == 0)
            {
                xPosition = Random.Range(area1Boundaries[0], area1Boundaries[1]);
                zPosition = Random.Range(area1Boundaries[2], area1Boundaries[3]);
            }
            else if (area == 1)
            {
                xPosition = Random.Range(area2Boundaries[0], area2Boundaries[1]);
                zPosition = Random.Range(area2Boundaries[2], area2Boundaries[3]);
            }
            else
            {
                xPosition = Random.Range(area3Boundaries[0], area3Boundaries[1]);
                zPosition = Random.Range(area3Boundaries[2], area3Boundaries[3]);
            }

            Instantiate(rock, new Vector3(xPosition, 40.0f, zPosition), Quaternion.identity);
        }
    }
}
