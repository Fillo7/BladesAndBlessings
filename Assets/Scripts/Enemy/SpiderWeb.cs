using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    private bool playerImmobilized = false;
    private PlayerMovement movement;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movement = other.GetComponent<PlayerMovement>();
            movement.LimitSpeed(0.0f);
            playerImmobilized = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movement.LimitSpeed(0.0f);
        }
    }

    void OnDestroy()
    {
        if (playerImmobilized)
        {
            movement.ResetSpeed(0.0f);
        }
    }
}
