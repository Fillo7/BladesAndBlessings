using UnityEngine;

public class Abberation : MonoBehaviour {

	private Transform player;
	private PlayerHealth playerHealth;
	private EnemyHealth enemyHealth;

	private UnityEngine.AI.NavMeshAgent navigator;

	[SerializeField] private float speed = 0.9f;
	[SerializeField] private float auraRadius = 3.5f;
	[SerializeField] private int auraDamage = 1;
	[SerializeField] private float tickTime = 0.2f;
	private float tickTimer = 0.0f;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();
		navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
		navigator.speed = speed;
	}

	void Update()
	{
		tickTimer += Time.deltaTime;

		if (enemyHealth.IsDead() || playerHealth.IsDead())
		{
			navigator.enabled = false;
			return;
		}

        if (IsPlayerInRange(auraRadius))
        {
            navigator.speed = 0.2f;

            if (tickTimer > tickTime)
            {
                playerHealth.TakeDamage(auraDamage);
                tickTimer = 0.0f;
            }
        }
        else
        {
            navigator.speed = speed;
        }

		if (navigator.enabled)
		{
			navigator.SetDestination(player.position);
		}
	}

	private bool IsPlayerInRange(float range)
	{
		return Vector3.Distance(transform.position, player.position) < range;
	}
}
