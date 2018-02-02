using UnityEngine;
using UnityEngine.AI;

public class GoblinPiker : MonoBehaviour
{
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private NavMeshAgent navigator;
    private Animator animator;

    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float attackRange = 1.8f;
    [SerializeField] private float attackCooldown = 2.0f;
    private float attackTimer = 1.0f;
    private bool attacking = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        navigator = GetComponent<NavMeshAgent>();
        navigator.speed = speed;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            navigator.enabled = false;
            return;
        }

        attackTimer += Time.deltaTime;

        if (navigator.enabled && navigator.velocity.magnitude > 0.1f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (IsPlayerInRange(attackRange))
        {
            navigator.enabled = false;
        }
        else
        {
            navigator.enabled = true;
        }

        if (IsPlayerInRange(attackRange) && IsPlayerInFront(60.0f) && attackTimer > attackCooldown && !attacking)
        {
            Attack();
        }

        if (attacking)
        {
            navigator.enabled = false;
        }
        else
        {
            navigator.enabled = true;
        }

        if (navigator.enabled)
        {
            navigator.SetDestination(player.position);
        }
    }

    private void Attack()
    {
        attacking = true;
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", 2.0f);
    }

    private void ResetAttack()
    {
        attackTimer = 0.0f;
        attacking = false;
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) < range;
    }

    private bool IsPlayerInFront(float range)
    {
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        return Mathf.Abs(angle) < range;
    }
}
