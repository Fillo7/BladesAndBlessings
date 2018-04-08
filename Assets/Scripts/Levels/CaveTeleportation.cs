using System.Collections.Generic;
using UnityEngine;

public class CaveTeleportation : MonoBehaviour
{
    [SerializeField] private List<Transform> centers = new List<Transform>();

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(100.0f);

            Transform targetArea = GetClosestCenter(other.transform.position);
            other.transform.SetPositionAndRotation(targetArea.position, Quaternion.identity);
        }
    }

    Transform GetClosestCenter(Vector3 position)
    {
        float shortestDistance = float.MaxValue;
        Transform result = null;

        foreach (Transform center in centers)
        {
            float currentDistance = Vector3.Distance(position, center.position);

            if (currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                result = center;
            }
        }

        return result;
    }
}
