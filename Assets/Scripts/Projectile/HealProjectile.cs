using UnityEngine;

public class HealProjectile : Projectile
{
    [SerializeField] private float healing = 20.0f;

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

    public override void SetOwner(ProjectileOwner owner)
    {
        if (owner == ProjectileOwner.Player)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Heal(healing);
            Destroy(gameObject);
        }
    }

    public void SetHealing(float healing)
    {
        this.healing = healing;
    }
}
