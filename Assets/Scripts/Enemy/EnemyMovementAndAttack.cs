using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementAndAttack : MonoBehaviour {

	//Attack CD
	public float timeBetweenAttacks = 2.0f;

	//Range
	public float range = 0.0f;

	//Damage
	public int damage = 4;

	//Speed
	public float movementSpeed = 1.0f;

	//Buffer distance - a.k.a. when ranged units start running instead of shooting
	public float bufferDistance = 0.25f;

	//Is unit ranged
	public bool isRanged = false;

	//Projectile which ranged unit shoots
	public GameObject[] projectile;

	//Is unit static - can the unit move
	public bool isStatic = false;

	//Is unit chasing - will the unit follow player
	public bool isChasing = true;

	//Waypoints for non-chasing unit
	public GameObject[] waypoints;

	int nextWaypoint = 0;

	float attackTimer = 0.0f;

	//Mimimal required distance from waypoint that needs to be reached before moving to next waypoint
	public float minimalDistanceFromWaypoint = 1.0f;

	//Player object
	GameObject player;

	void Awake () {

		player = GameObject.FindGameObjectWithTag("Player");

	}

	void Update () {

		//we check whether the unit is ranged
		//non ranged units move whenever they can
		//ranged units move only when they need to - when their target is either too far or too close
		if (isRanged) {

			//is too close or too far?
			if ((Vector3.Distance (gameObject.transform.position, player.transform.position) < bufferDistance) || (Vector3.Distance (gameObject.transform.position, player.transform.position) > range * 0.8f)) {

				Move ();
				//when ranged unit moves, we reset its attack timer
				attackTimer = 0.0f;

			} else {

				attackTimer += Time.deltaTime;

			}

		} else {

			Move ();
			attackTimer += Time.deltaTime;

		}

		//if the unit is ready to attack, we let it attack
		if (attackTimer > timeBetweenAttacks) {

			attackTimer -= timeBetweenAttacks;
			Attack ();

		}

	}

	void Move() {

		//if the unit is not static, we move it
		if (!isStatic) {

			//if it is chasing, we move it towards the player
			if (isChasing) {

				gameObject.transform.LookAt(player.transform.position);

			//otherwise we move it towards the next waypoint
			} else {

				//we calculate whether we are close enough to current waypoint - if so, we switch to the next one
				if (Vector3.Distance (gameObject.transform.position, waypoints[nextWaypoint].transform.position) < minimalDistanceFromWaypoint) {

					if (nextWaypoint + 1 >= waypoints.Length) {

						nextWaypoint = 0;

					} else {

						nextWaypoint++;

					}

				}

				gameObject.transform.LookAt(waypoints[nextWaypoint].transform.position);

			}

			gameObject.transform.position += gameObject.transform.forward * movementSpeed * Time.deltaTime;

		}

	}

	void Attack() {

		gameObject.transform.LookAt (player.transform.position);

		//if our unit has ranged attack, we spawn the projectile towards the player
		//othervise we perform meele attack
		if (isRanged) {

			Instantiate (projectile[UnityEngine.Random.Range(0,projectile.Length - 1)], gameObject.transform.position, gameObject.transform.rotation);

		} else {

			PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
			playerHealth.TakeDamage(damage);

		}

	}

}
