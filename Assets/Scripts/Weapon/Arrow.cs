using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage;
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

        Destroy(gameObject);
    }

    public void SetDamage(int value)
    {
        damage = value;
    }
}
