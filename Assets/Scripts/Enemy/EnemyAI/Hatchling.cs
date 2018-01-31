using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatchling : MonoBehaviour {

	private Transform player;
	private PlayerHealth playerHealth;
	private EnemyHealth enemyHealth;

	[SerializeField] private float movementSpeed = 2.0f;

	[SerializeField] private float attackCooldown = 3.0f;

	[SerializeField] private float attackRange = 1.8f;

	[SerializeField] private float damage = 5;

	private float attackTimer = 0.0f;

	private UnityEngine.AI.NavMeshAgent navigator;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();

		navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
		navigator.speed = movementSpeed;

	}

	void Update()
	{

		attackTimer += Time.deltaTime;

		if (enemyHealth.GetCurrentHealth() <= 0 || playerHealth.IsDead())
		{
			navigator.enabled = false;
			return;
		}

		if (Vector3.Distance(transform.position, player.position) < attackRange && attackTimer > attackCooldown)
		{
			playerHealth.TakeDamage (damage);
			attackTimer = 0;
		}

		if (navigator.enabled)
		{
			navigator.SetDestination(player.position);
		}
	}
		
}
