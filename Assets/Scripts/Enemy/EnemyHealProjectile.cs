using UnityEngine;

public class EnemyHealProjectile : Projectile {

	//for how much this object heals any enemy that is in range
	[SerializeField] private float heal = 0.0f;

	public void SetHeal(float heal)
	{
		this.heal = heal;
	}

	void OnTriggerEnter(Collider other)
	{
        if (other.tag.Equals("Projectile") || other.tag.Equals("Weapon") || other.tag.Equals("EnemyObject") || other.tag.Equals("Player"))
        {
            return;
        }

        if (other.tag.Equals("Enemy") && owner == ProjectileOwner.Enemy)
		{

			EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
			enemyHealth.Heal (heal);
		}

        Destroy(gameObject);
    }

}
