using UnityEngine;

public class HealProjectile : Projectile
{
    [SerializeField] private float healing = 20.0f;

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
                EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                enemyHealth.Heal(healing);
            }
            else
            {
                return;
            }
        }
        else if (other.tag.Equals("Player"))
        {
            return;
        }

        Destroy(gameObject);
    }

    public void SetHealing(float healing)
    {
        this.healing = healing;
    }
}
