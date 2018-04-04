﻿using System.Collections.Generic;
using UnityEngine;

public class ForestRuins : MonoBehaviour
{
    [SerializeField] private GameObject spider;
    [SerializeField] private List<Transform> waypoints;

    private LinkedList<GameObject> activeSpiders = new LinkedList<GameObject>();
    private float timer = 20.0f;
    private float nextSpawn = 40.0f;

    void Update()
    {
        timer += Time.deltaTime;
        UpdateActiveSpiders();

        if (timer >= nextSpawn)
        {
            timer = Random.Range(0.0f, 10.0f);

            if (activeSpiders.Count >= 4)
            {
                return;
            }

            GameObject spawnedSpider = Instantiate(spider, waypoints[Random.Range(0, waypoints.Count)].position, Quaternion.identity);
            spawnedSpider.GetComponent<Spider>().InitializeWaypoints(waypoints);
            activeSpiders.AddLast(spawnedSpider);
        }
    }

    private void UpdateActiveSpiders()
    {
        LinkedListNode<GameObject> currentNode = activeSpiders.First;
        while (currentNode != null)
        {
            LinkedListNode<GameObject> next = currentNode.Next;
            if (currentNode.Value == null)
            {
                activeSpiders.Remove(currentNode);
            }

            currentNode = next;
        }
    }
}
