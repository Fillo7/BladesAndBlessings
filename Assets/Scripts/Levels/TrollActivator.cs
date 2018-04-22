using System.Collections.Generic;
using UnityEngine;

public class TrollActivator : MonoBehaviour
{
    [SerializeField] private Troll troll;
    [SerializeField] private List<CavePlatformController> cavePlatformsToDestroy;

    private bool trollActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !trollActivated)
        {
            ActivateTroll();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player") && !trollActivated)
        {
            ActivateTroll();
        }
    }

    private void ActivateTroll()
    {
        trollActivated = true;
        troll.SetActive();
        Invoke("DestroyPlatforms", 1.5f);
    }

    private void DestroyPlatforms()
    {
        foreach (CavePlatformController platform in cavePlatformsToDestroy)
        {
            platform.Destroy();
        }

        gameObject.SetActive(false);
    }
}
