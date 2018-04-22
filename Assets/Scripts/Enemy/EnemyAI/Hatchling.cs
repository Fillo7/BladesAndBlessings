using UnityEngine;
using UnityEngine.AI;

public class Hatchling : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float damage = 10;
    [SerializeField] private float attackCooldown = 1.5f;

    private Animator animator;
    private NavMeshObstacle obstacle;

    private float attackRange = 1.75f;
    private float attackTimer = 1.5f;
    private bool attacking = false;
    private bool playerHit = false;

    protected override void Awake()
    {
        base.Awake();
        navigator.speed = movementSpeed;
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("AttackSpeedMultiplier", 1.5f);
        animator.SetFloat("IdleSpeedMultiplier", 6.5f);
        animator.SetFloat("DeathSpeedMultiplier", 0.65f);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !playerHit)
        {
            playerHit = true;
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            return;
        }

        attackTimer += Time.deltaTime;

        if (IsPlayerInRange(attackRange))
        {
            navigator.enabled = false;
            obstacle.enabled = true;

            if (IsPlayerInFront(60.0f) && attackTimer > attackCooldown && !attacking)
            {
                Attack();
            }
            TurnTowardsPlayer(220.0f);
        }
        else
        {
            if (!attacking)
            {
                obstacle.enabled = false;
                navigator.enabled = true;
            }
        }

        if (navigator.enabled)
        {
            navigator.SetDestination(player.position);
        }
    }

    public void ResetAttack()
    {
        attackTimer = 0.0f;
        obstacle.enabled = false;
        navigator.enabled = true;
        attacking = false;
    }

    private void Attack()
    {
        attacking = true;
        playerHit = false;
        navigator.enabled = false;
        obstacle.enabled = true;
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length / 1.5f);
    }
}
