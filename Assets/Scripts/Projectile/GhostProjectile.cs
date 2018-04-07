using UnityEngine;

public class GhostProjectile : Projectile
{
    [SerializeField] private float damage = 30.0f;
    [SerializeField] private DamageType damageType = DamageType.Piercing;
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float slowDuration = 5.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            if (owner == ProjectileOwner.Enemy)
            {
                return;
            }

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage, damageType);
            Destroy(gameObject);
        }
        else if (other.tag.Equals("Player"))
        {
            if (owner == ProjectileOwner.Player)
            {
                return;
            }

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.ApplyMovementEffect(new MovementEffect(slowDuration, slowMultiplier));
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
