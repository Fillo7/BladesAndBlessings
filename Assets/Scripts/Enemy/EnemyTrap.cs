using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour {

	//for how long will the trap snare the player when he speps on it
	public float lengthOfSnare = 2.0f;

	//how much damage deals the trap to the player when he steps on it
	public int damage = 0;

	//how long (in seconds) will the trap exist until it despawns
	public float timeToLive = 15.0f;

	GameObject player;

	void Awake () {

		player = GameObject.FindGameObjectWithTag("Player");

	}

	void Update () {

		timeToLive -= Time.deltaTime;

		if (timeToLive < 0.0f) {

			Destroy (gameObject);

		}

	}

	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject == player)
		{

			PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth> ();
			playerHealth.TakeDamage (damage);
			PlayerMovement playerMovement = player.gameObject.GetComponent<PlayerMovement> ();
			playerMovement.Snare (lengthOfSnare);
			Destroy (gameObject);

		}

	}

}
