using UnityEngine;

public class GoblinBehaviour : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent navigator;
    private Animator animator;

    float attackTimer = 1.5f;
    float attackCooldown = 1.2f;
    float attackRange = 2.5f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (enemyHealth.GetCurrentHealth() <= 0 || playerHealth.IsDead())
        {
            navigator.enabled = false;
            return;
        }

        if (IsPlayerInRange(attackRange))
        {
            navigator.enabled = false;
        }
        else
        {
            navigator.enabled = true;
        }

        if (IsPlayerInRange(attackRange) && IsPlayerInFront(60.0f) && attackTimer > attackCooldown)
        {
            Attack();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PikeAttack"))
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
        animator.SetTrigger("Attack");
        attackTimer = 0.0f;
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
