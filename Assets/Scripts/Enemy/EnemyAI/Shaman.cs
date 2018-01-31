using UnityEngine;

public class Shaman : MonoBehaviour {

	private Transform player;
	private PlayerHealth playerHealth;
	private EnemyHealth enemyHealth;

	[SerializeField] private GameObject fireball;
	[SerializeField] private int fireballDamage = 20;
	[SerializeField] private GameObject healingball;
	[SerializeField] private int healingballHeal = 40;

	[SerializeField] private float movementSpeed = 2.0f;

	[SerializeField] private float minimumDistance = 6.0f;

	[SerializeField] private float maximumDistance = 12.0f;

	[SerializeField] private float attackCooldown = 3.0f;
	private float attackTimer = 0.0f;

	private bool isRelocating = true;

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

		if (enemyHealth.GetCurrentHealth() <= 0 || playerHealth.IsDead())
		{
			navigator.enabled = false;
			return;
		}

		if (isRelocating && DistanceToPlayer () > (maximumDistance - minimumDistance) * 0.25 + minimumDistance && DistanceToPlayer () < (maximumDistance - minimumDistance) * 0.75 + minimumDistance)
		{
			isRelocating = false;
			navigator.enabled = false;
		}

		if (DistanceToPlayer () > maximumDistance)
		{
			attackTimer = 0f;
			isRelocating = true;
			navigator.enabled = true;
			navigator.SetDestination(player.position);

		}

		if (DistanceToPlayer () < minimumDistance)
		{
			attackTimer = 0f;
			isRelocating = true;	
			navigator.enabled = true;
			transform.LookAt(player.transform);
			navigator.SetDestination(transform.position - transform.forward * 10f);
		}
		if (!isRelocating)
		{
			transform.LookAt(player.transform);
			attackTimer += Time.deltaTime;
		}

		if (attackTimer > attackCooldown && IsPlayerInSight() && !isRelocating)
		{
			attackTimer -= attackCooldown;
			CastSpell();
		}
	}

	private void CastSpell()
	{
		Vector3 spellDirection = player.position - transform.position;
		transform.LookAt(player.transform);
		if (Random.Range (0, 2) > 0) 
		{
			GameObject fireballInstance = Instantiate(fireball, transform.position + transform.forward * 2f + transform.up * 0.8f,
				Quaternion.LookRotation(spellDirection, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
			Arrow script = fireballInstance.GetComponent<Arrow>();
			script.SetDamage(fireballDamage);
			script.SetOwner(ProjectileOwner.Enemy);
			script.SetDirection(fireballInstance.transform.up);
		} 
		else 
		{
			GameObject healingballInstance = Instantiate(healingball, transform.position + transform.forward * 2f + transform.up * 0.8f,
				Quaternion.LookRotation(spellDirection, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
			EnemyHealProjectile script = healingballInstance.GetComponent<EnemyHealProjectile>();
			script.SetHeal(healingballHeal);
			script.SetOwner(ProjectileOwner.Enemy);
			script.SetDirection(healingballInstance.transform.forward);
		}

	}

	private bool IsPlayerInSight()
	{
		RaycastHit hit;
		Vector3 rayDirection = player.position - transform.position;

		if (Physics.Raycast(transform.position, rayDirection, out hit))
		{
			return hit.transform == player;
		}
		return false;
	}

	private float DistanceToPlayer()
	{
		return Vector3.Distance(transform.position, player.position);
	}
		
}
