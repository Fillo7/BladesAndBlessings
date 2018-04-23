using UnityEngine;

public class TrollBreath : MonoBehaviour
{
    private float breathDamage = 5.0f;

    private float tickTime = 0.25f;
    private float tickTimer = 0.0f;

    private bool playerInBreath = false;

    private PlayerHealth playerHealth;
    private Rigidbody playerBody;

    void Update()
    {
        tickTimer += Time.deltaTime;

        if (tickTimer > tickTime)
        {
            ApplyBreath();
            tickTimer = 0.0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            playerBody = other.GetComponent<Rigidbody>();
            playerInBreath = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            playerInBreath = false;
        }
    }

    private void ApplyBreath()
    {
        if (playerInBreath)
        {
            playerHealth.TakeDamage(breathDamage);
            playerBody.AddForce(transform.forward * 5000.0f);
        }
    }
}
