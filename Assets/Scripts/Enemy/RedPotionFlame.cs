using UnityEngine;

public class RedPotionFlame : MonoBehaviour
{
    [SerializeField] private float flameDuration = 4.1f;
    [SerializeField] private float tickTime = 0.5f;
    [SerializeField] private float flameDamage = 10.0f;
    
    private float timer = 0.0f;
    private float tickTimer = 0.0f;

    private bool playerInside = false;
    private PlayerHealth playerHealth = null;

    void Update()
    {
        timer += Time.deltaTime;
        tickTimer += Time.deltaTime;

        if (tickTimer > tickTime)
        {
            if (playerInside)
            {
                playerHealth.TakeDamage(flameDamage);
            }
            tickTimer = 0.0f;
        }

        if (timer >= flameDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerInside = true;
            playerHealth = other.GetComponent<PlayerHealth>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerInside = false;
        }
    }
}
