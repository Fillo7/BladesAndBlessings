using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 20;
    private float speed = 20.0f;
    private float timeToLive = 10.0f;
    private ProjectileOwner owner = ProjectileOwner.Player;

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
        if (other.tag.Equals("Projectile") || other.tag.Equals("Weapon"))
        {
            return;
        }

        if (other.tag.Equals("Enemy"))
        {
            if (owner == ProjectileOwner.Enemy)
            {
                return;
            }

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
        }
        else if (other.tag.Equals("Player"))
        {
            if (owner == ProjectileOwner.Player)
            {
                return;
            }

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
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

    public void SetOwner(ProjectileOwner owner)
    {
        this.owner = owner;
    }

    public void FollowDirection(Vector3 direction)
    {
        GetComponent<Rigidbody>().velocity = direction * speed;
    }

    public void SwapDirection()
    {
        Vector3 newVelocity = GetComponent<Rigidbody>().velocity;
        newVelocity = Quaternion.Euler(0.0f, 180.0f, 0.0f) * newVelocity;
        GetComponent<Rigidbody>().velocity = newVelocity;

        transform.rotation = Quaternion.LookRotation(newVelocity, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
    }
}
