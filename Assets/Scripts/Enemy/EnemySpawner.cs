using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	//Spawn time
	public float spawnTime = 10.0f;

	//Number of spawn charges
	public int numberOfCharges = 1;

	//Self destruct
	public bool selfDestruct = true;

	//Enemy to spawn
	public GameObject enemy = null;

	float time = 0.0f;

	void Update () {

		time += Time.deltaTime;

		if ((time > spawnTime) && (numberOfCharges > 0)) {

			time -= spawnTime;

			spawnEnemy ();

			numberOfCharges -= 1;

			if ((selfDestruct) && (numberOfCharges <= 0)) {
			
				Destroy (gameObject);

			}

		}

	}

	void spawnEnemy() {

		Instantiate(enemy, gameObject.transform.position, gameObject.transform.rotation);

	}
}
