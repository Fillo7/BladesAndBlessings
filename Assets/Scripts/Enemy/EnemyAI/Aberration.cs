using UnityEngine;
using UnityEngine.AI;

public class Aberration : MonoBehaviour {

	private Transform player;
	private PlayerHealth playerHealth;
	private EnemyHealth enemyHealth;

    private Animator animator;
    private NavMeshAgent navigator;

	[SerializeField] private float speed = 1.0f;
	[SerializeField] private float auraRadius = 3.5f;
	[SerializeField] private float auraDamage = 1.5f;
	[SerializeField] private float tickTime = 0.2f;
	private float tickTimer = 0.0f;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
		enemyHealth = GetComponent<EnemyHealth>();

        animator = GetComponentInChildren<Animator>();
        navigator = GetComponent<NavMeshAgent>();
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

        if (navigator.enabled && navigator.velocity.magnitude > 0.25f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
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
