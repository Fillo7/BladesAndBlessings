using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageAura : MonoBehaviour {

	//how much damage the aura deals 
	public int damagePerTick = 1;

	//time in seconds between two ticks
	public float tickLength = 0.4f;

	//radius of the aura aroung the character that has it
	public float auraRadius = 4.0f;

	GameObject player;

	float tickTimer = 0.0f;

	void Awake () {

		player = GameObject.FindGameObjectWithTag("Player");

	}

	void Update () {

		tickTimer += Time.deltaTime;

		if (tickTimer > tickLength) {

			tickTimer -= tickLength;

			if (Vector3.Distance (gameObject.transform.position, player.transform.position) < auraRadius) {

				PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
				playerHealth.TakeDamage(damagePerTick);

			}

		}

	}
}
