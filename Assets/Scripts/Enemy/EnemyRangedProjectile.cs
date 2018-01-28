using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedProjectile : MonoBehaviour {

	//how much damage this object deals to player when it hits him
	public int damage = 0;

	//how fast it flies
	public float speed = 1.0f;

	//how many seconds the projectile can fly
	public float timeToLive = 1.0f;

	GameObject player;

	void Awake () {

		player = GameObject.FindGameObjectWithTag("Player");

	}

	void Update () {

		if (timeToLive <= 0.0f) {
		
			Destroy (gameObject);
		
		}

		gameObject.transform.position += gameObject.transform.forward * speed * Time.deltaTime;

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject == player)
		{
			
			PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
			playerHealth.TakeDamage(damage);
			Destroy (gameObject);

		}

	}

}
