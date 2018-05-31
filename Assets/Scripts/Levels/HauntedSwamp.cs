using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HauntedSwamp : MonoBehaviour
{
    [SerializeField] private GameObject snake;
    [SerializeField] private List<ListWrapper> snakePaths;

    private LinkedList<Tuple<GameObject, int>> activeSnakes = new LinkedList<Tuple<GameObject, int>>();
    private float timer = 20.0f;
    private float nextSpawn = 35.0f;

    void Update()
    {
        timer += Time.deltaTime;
        UpdateActiveSnakes();

        if (timer >= nextSpawn)
        {
            timer = 0.0f;

            if (activeSnakes.Count >= 3)
            {
                return;
            }

            int pathIndex = GetUnusedPathIndex();

            GameObject spawnedSnake = Instantiate(snake, snakePaths[pathIndex].internalList[0].position, Quaternion.identity);
            spawnedSnake.GetComponent<Snake>().InitializeWaypoints(snakePaths[pathIndex].internalList);
            activeSnakes.AddLast(new Tuple<GameObject, int>(spawnedSnake, pathIndex));
        }
    }

    private void UpdateActiveSnakes()
    {
        LinkedListNode<Tuple<GameObject, int>> currentNode = activeSnakes.First;
        while (currentNode != null)
        {
            LinkedListNode<Tuple<GameObject, int>> next = currentNode.Next;
            if (currentNode.Value.First == null)
            {
                activeSnakes.Remove(currentNode);
            }

            currentNode = next;
        }
    }

    private int GetUnusedPathIndex()
    {
        List<int> unusedIndices = new List<int>();

        for (int i = 0; i < snakePaths.Count; i++)
        {
            unusedIndices.Add(i);
        }

        List<int> usedIndices = new List<int>();
        LinkedListNode<Tuple<GameObject, int>> currentNode = activeSnakes.First;
        while (currentNode != null)
        {
            LinkedListNode<Tuple<GameObject, int>> next = currentNode.Next;
            usedIndices.Add(currentNode.Value.Second);
            currentNode = next;
        }

        List<int> result = unusedIndices.Except(usedIndices).ToList();
        return result[Random.Range(0, result.Count)];
    }
}
