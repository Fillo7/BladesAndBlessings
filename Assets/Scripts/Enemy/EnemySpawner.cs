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

	private float timer = 0.0f;

	private EnemyHealth health;

	void Awake()
	{
		health = GetComponent<EnemyHealth>();
	}

	void Update () {

		if (health.IsDead())
		{
			return;
		}

		timer += Time.deltaTime;

		if ((timer > spawnTime) && (numberOfCharges > 0)) {

			timer -= spawnTime;

			SpawnEnemy ();

			numberOfCharges -= 1;

			if ((selfDestruct) && (numberOfCharges <= 0)) {
			
				Destroy (gameObject);

			}

		}

	}

	void SpawnEnemy() {

		Instantiate(enemy, gameObject.transform.position, gameObject.transform.rotation);

	}
}
