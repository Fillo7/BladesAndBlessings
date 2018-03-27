using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] private float lengthOfSnare = 3.0f;
    [SerializeField] private float damage = 25.0f;
    [SerializeField] private float timeToLive = 30.0f;

    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.ApplyMovementEffect(lengthOfSnare, 0.0f);
            Destroy(gameObject);
        }
    }
}
