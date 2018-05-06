using UnityEngine;
using UnityEngine.AI;

public class GoblinRogue : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float damage = 15.0f;
    [SerializeField] private float attackCooldown = 2.0f;
    [SerializeField] private float invisibilityCooldown = 8.0f;

    private Animator animator;
    private GoblinDagger weapon;
    private NavMeshObstacle obstacle;
    private EnemyHealth health;

    private float minimumPlayerDistance = 18.0f;
    private float attackRange = 1.7f;
    private float attackTimer = 2.0f;
    private float invisibilityTimer = 4.0f;
    private bool attacking = false;
    private bool invisible = false;
    private int healthSnapshot = 0;
    private bool stunned = false;
    private float stunTimer = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<GoblinDagger>();
        weapon.Initialize(animator, this, damage);
        GetComponentInChildren<EnemyWeaponDelegate>().SetWeapon(weapon);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        health = GetComponent<EnemyHealth>();
        healthSnapshot = health.GetCurrentHealth();
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            TurnVisible();
            navigator.enabled = false;
            obstacle.enabled = false;
            return;
        }

        if (stunned)
        {
            navigator.enabled = false;
            stunTimer -= Time.deltaTime;

            if (stunTimer <= 0.0f)
            {
                stunned = false;
                navigator.enabled = true;
            }

            return;
        }

        attackTimer += Time.deltaTime;
        if (!invisible)
        {
            invisibilityTimer += Time.deltaTime;
        }

        if (navigator.enabled && navigator.velocity.magnitude > 0.35f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (invisibilityTimer > invisibilityCooldown && !invisible)
        {
            TurnInvisible();
            invisibilityTimer = 0.0f;
        }

        if (healthSnapshot != health.GetCurrentHealth() && invisible)
        {
            TurnVisible();
        }
        healthSnapshot = health.GetCurrentHealth();

        if (invisible)
        {
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
        else
        {
            if (navigator.destination == player.position)
            {
                navigator.SetDestination(transform.position);
            }

            if (GetDistanceToPlayer() < minimumPlayerDistance && !attacking)
            {
                obstacle.enabled = false;
                navigator.enabled = true;

                Vector3 fleeDirection = -(player.position - transform.position);
                navigator.SetDestination(fleeDirection.normalized * 15.0f);
            }
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

    public void TurnVisible()
    {
        invisible = false;
        navigator.speed = 5.0f;
        SetVisibility(true);
    }

    public void ApplyStun(float duration)
    {
        stunned = true;
        stunTimer = duration;
    }

    private void TurnInvisible()
    {
        invisible = true;
        navigator.speed = 3.0f;
        SetVisibility(false);
    }

    private void SetVisibility(bool flag)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = flag;
        }

        SkinnedMeshRenderer[] skinnedRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < skinnedRenderers.Length; i++)
        {
            skinnedRenderers[i].enabled = flag;
        }
    }

    private void Attack()
    {
        attacking = true;
        navigator.enabled = false;
        obstacle.enabled = true;
        Invoke("TurnVisible", 0.25f);
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length);
    }
}
