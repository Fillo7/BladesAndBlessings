using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private float damage = 20.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Projectile") || other.tag.Equals("Weapon") || other.tag.Equals("EnemyObject"))
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

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
