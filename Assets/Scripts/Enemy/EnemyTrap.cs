using UnityEngine;

public class EnemyTrap : MonoBehaviour {

    //for how long will the trap snare the player when he speps on it
    [SerializeField] private float lengthOfSnare = 3.0f;

    //how much damage deals the trap to the player when he steps on it
    [SerializeField] private float damage = 25.0f;

    //how long (in seconds) will the trap exist until it despawns
    [SerializeField] private float timeToLive = 30.0f;

    void Update () {

        timeToLive -= Time.deltaTime;

        if (timeToLive < 0.0f) {

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
