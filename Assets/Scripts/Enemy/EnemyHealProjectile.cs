using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealProjectile : MonoBehaviour {

	//for how much this object heals any enemy that is in range
	public int heal = 0;

	//how fast it flies
	public float speed = 1.0f;

	//how many seconds the projectile can fly
	public float timeToLive = 1.0f;

	void Update () {

		if (timeToLive <= 0.0f) {

			Destroy (gameObject);

		}

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
