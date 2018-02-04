using UnityEngine;
using UnityEngine.AI;

public class Aberration : MonoBehaviour {

    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private Animator animator;
    private NavMeshAgent navigator;

    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float auraRadius = 4.0f;
    [SerializeField] private float auraDamage = 8.0f;
    [SerializeField] private float tickTime = 0.25f;
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
            tickTimer += Time.deltaTime;
            navigator.speed = 0.2f;

            if (tickTimer > tickTime)
            {
                playerHealth.TakeDamage(auraDamage);
                tickTimer = 0.0f;
            }
        }
        else
        {
            tickTimer = 0.0f;
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
