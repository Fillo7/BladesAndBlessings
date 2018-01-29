using UnityEngine;

public class EnemyHealProjectile : Projectile {

	//for how much this object heals any enemy that is in range
	public int heal = 0;

	protected override void Update () {
        base.Update();

		gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.tag == "Enemy")
		{

			EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
			enemyHealth.Heal (heal);

		}

	}

}
