using UnityEngine;
using UnityEngine.AI;

public class Hatchling : MonoBehaviour {

	private Transform player;
	private PlayerHealth playerHealth;
	private EnemyHealth enemyHealth;

	[SerializeField] private float movementSpeed = 2.0f;

	[SerializeField] private float attackCooldown = 3.0f;

	[SerializeField] private float attackRange = 1.9f;

	[SerializeField] private float damage = 5;

	private float attackTimer = 0.0f;

	private NavMeshAgent navigator;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();

		navigator = GetComponent<NavMeshAgent>();
		navigator.speed = movementSpeed;

	}

	void Update()
	{

		attackTimer += Time.deltaTime;

		if (enemyHealth.IsDead() || playerHealth.IsDead())
		{
			navigator.enabled = false;
			return;
		}

		if (Vector3.Distance(transform.position, player.position) < attackRange)
		{
			navigator.speed = 0.05f;

			if (attackTimer > attackCooldown)
			{
				playerHealth.TakeDamage(damage);
				attackTimer = 0.0f;
			}
		}
		else
		{
			navigator.speed = movementSpeed;
		}

		if (navigator.enabled)
		{
			navigator.SetDestination(player.position);
		}
	}
		
}
