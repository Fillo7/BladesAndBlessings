using UnityEngine;
using UnityEngine.AI;

public class Hatcher : MonoBehaviour {

	private PlayerHealth playerHealth;
	private EnemyHealth enemyHealth;

	private NavMeshAgent navigator;

	[SerializeField] private float speed = 0.9f;

	void Awake()
	{
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();
		navigator = GetComponent<NavMeshAgent>();
		navigator.speed = speed;
		navigator.SetDestination(GetRandomPosition(20f));
	}

	void Update()
	{

		navigator.enabled = true;

		if (enemyHealth.IsDead() || playerHealth.IsDead())
		{
			navigator.enabled = false;
			return;
		}

		if (navigator.enabled && DistanceToTarget() < 0.4f)
		{
			navigator.SetDestination(GetRandomPosition(20f));
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Wall"))
		{
			navigator.SetDestination(transform.position - transform.forward * 6f);
		}
	}

	private Vector3 GetRandomPosition(float maximumDistance)
	{
		Vector3 randomDirection = Random.insideUnitSphere * maximumDistance;
		randomDirection += transform.position;

		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, maximumDistance, -1);

		return hit.position;
	}

	private float DistanceToTarget()
	{
		return Vector3.Distance(transform.position, navigator.destination);
	}

}
