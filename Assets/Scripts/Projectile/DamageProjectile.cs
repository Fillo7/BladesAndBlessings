using UnityEngine;

public class DamageProjectile : Projectile
{
    [SerializeField] private float damage = 20.0f;
    [SerializeField] private DamageType damageType = DamageType.Piercing;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Projectile") || other.tag.Equals("Weapon") || other.tag.Equals("EnemyObject") || other.tag.Equals("Wall") || (other.isTrigger && !other.tag.Equals("Enemy")))
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
            enemyHealth.TakeDamage(damage, damageType);
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
