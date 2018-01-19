using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 20;
    private float speed = 20.0f;
    private float timeToLive = 10.0f;

    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            return;
        }

        if (other.tag.Equals("Enemy"))
        {
            EnemyHealthController enemyHealth = other.GetComponent<EnemyHealthController>();
            enemyHealth.TakeDamage(damage);
        }
        else if (other.tag.Equals("Player"))
        {
            PlayerHealthController playerHealth = other.GetComponent<PlayerHealthController>();
            playerHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void FollowDirection(Vector3 direction)
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
    }
}
