using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] private int damage = 20;

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

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
