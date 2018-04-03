using System.Collections.Generic;
using UnityEngine;

public class ForestRuins : MonoBehaviour
{
    [SerializeField] private GameObject spider;
    [SerializeField] private List<ListWrapper> pathList;

    private LinkedList<Tuple<int, GameObject>> activePaths = new LinkedList<Tuple<int, GameObject>>();
    private float timer = 20.0f;
    private float nextSpawn = 35.0f;

    void Update()
    {
        timer += Time.deltaTime;
        UpdateActivePaths();

        if (timer >= nextSpawn)
        {
            timer = Random.Range(0.0f, 10.0f);
            int index = GetNextPathIndex();

            if (index == -1)
            {
                return;
            }

            GameObject spawnedSpider = Instantiate(spider, pathList[index].internalList[0].position, Quaternion.identity);
            spawnedSpider.GetComponent<Spider>().InitializeWaypoints(pathList[index].internalList);
            activePaths.AddLast(Tuple.New(index, spawnedSpider));
        }
    }

    private void UpdateActivePaths()
    {
        LinkedList<Tuple<int, GameObject>> toRemove = new LinkedList<Tuple<int, GameObject>>();

        foreach (Tuple<int, GameObject> item in activePaths)
        {
            if (item.Second == null)
            {
                toRemove.AddLast(item);
            }
        }

        foreach (Tuple<int, GameObject> item in toRemove)
        {
            activePaths.Remove(item);
        }
    }

    private int GetNextPathIndex()
    {
        List<int> unusedIndices = new List<int>();

        for (int i = 0; i < pathList.Count; i++)
        {
            bool indexUsed = false;

            foreach (Tuple<int, GameObject> item in activePaths)
            {
                if (item.First == i)
                {
                    indexUsed = true;
                    continue;
                }
            }

            if (!indexUsed)
            {
                unusedIndices.Add(i);
            }
        }

        if (unusedIndices.Count == 0)
        {
            return -1;
        }

        return unusedIndices[Random.Range(0, unusedIndices.Count)];
    }
}
