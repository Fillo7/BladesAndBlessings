using System.Collections.Generic;
using UnityEngine;

public class CaveTeleportation : MonoBehaviour
{
    [SerializeField] private List<CavePlatformController> platforms = new List<CavePlatformController>();

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(100.0f);

            Vector3 targetArea = GetClosestCenter(other.transform.position);
            other.transform.SetPositionAndRotation(targetArea, Quaternion.identity);
        }
    }

    Vector3 GetClosestCenter(Vector3 position)
    {
        float shortestDistance = float.MaxValue;
        Vector3 result = new Vector3();

        foreach (CavePlatformController platform in platforms)
        {
            if (!platform.IsActive())
            {
                continue;
            }

            Vector3 platformCenter = platform.GetCenterPoint();
            float currentDistance = Vector3.Distance(position, platformCenter);

            if (currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                result = platformCenter;
            }
        }

        return result;
    }
}
