using UnityEngine;
using UnityEngine.AI;

public class OrcWarchief : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float damage = 45.0f;
    [SerializeField] private float attackCooldown = 1.0f;

    private Animator animator;
    private WarchiefAxe weapon;
    private NavMeshObstacle obstacle;

    private float activeBlockingTimer = 0.0f;
    private float activeBlockingDuration = 10.0f;
    private float blockingTimer = 4.0f;
    private float blockingCooldown = 8.0f;
    private bool blocking = false;

    private float attackRange = 3.2f;
    private float attackTimer = 1.0f;
    private bool attacking = false;

    private float movementSpeedSnapshot;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("RunningSpeedMultiplier", 0.8f);
        weapon = GetComponentInChildren<WarchiefAxe>();
        weapon.Initialize(animator, this, damage);
        GetComponentInChildren<EnemyWeaponDelegate>().SetWeapon(weapon);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        movementSpeedSnapshot = movementSpeed;
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            obstacle.enabled = false;
            return;
        }

        attackTimer += Time.deltaTime;
        blockingTimer += Time.deltaTime;

        if (blockingTimer > blockingCooldown)
        {
            movementSpeed = 2.0f;
            blocking = true;
        }

        if (blocking)
        {
            activeBlockingTimer += Time.deltaTime;
            animator.SetBool("Blocking", true);

            if (activeBlockingTimer > activeBlockingDuration)
            {
                ResetBlocking();
            }
        }
        else
        {
            animator.SetBool("Blocking", false);
        }

        if (navigator.enabled && navigator.velocity.magnitude > 0.35f)
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
            obstacle.enabled = true;

            if (IsPlayerInFront(60.0f) && attackTimer > attackCooldown && !attacking)
            {
                Attack();
            }
            else
            {
                TurnTowardsPlayer();
            }
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

    public void SetAttackTimer(float attackTimer)
    {
        this.attackTimer = attackTimer;
    }

    public void ResetAttack()
    {
        attackTimer = 0.0f;
        obstacle.enabled = false;
        navigator.enabled = true;
        attacking = false;
    }

    public void ResetBlocking()
    {
        activeBlockingTimer = 0.0f;
        blockingTimer = 0.0f;
        movementSpeed = movementSpeedSnapshot;
        blocking = false;
    }

    private void Attack()
    {
        attacking = true;
        navigator.enabled = false;
        obstacle.enabled = true;
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length);
    }
}
