using UnityEngine;

public class TrapperTrap : MonoBehaviour
{
    [SerializeField] private AudioClip activationSound;
    private AudioSource audioPlayer;

    [SerializeField] private float lengthOfSnare = 3.0f;
    [SerializeField] private float damage = 25.0f;
    [SerializeField] private float timeToLive = 60.0f;

    private bool activated = false;

    void Awake()
    {
        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.clip = activationSound;
    }

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
        if (other.tag.Equals("Player") && !activated)
        {
            activated = true;
            audioPlayer.Play();
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.ApplyMovementEffect(new MovementEffect(lengthOfSnare, 0.0f));
            Destroy(gameObject, lengthOfSnare);
        }
    }
}
