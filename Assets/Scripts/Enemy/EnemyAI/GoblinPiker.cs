using UnityEngine;
using UnityEngine.AI;

public class GoblinPiker : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float damage = 30.0f;
    [SerializeField] private float attackCooldown = 1.25f;

    private Animator animator;
    private GoblinPike weapon;
    private NavMeshObstacle obstacle;

    private float attackRange = 1.8f;
    private float attackTimer = 1.25f;
    private bool attacking = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<GoblinPike>();
        weapon.Initialize(animator, this, damage);
        GetComponentInChildren<EnemyWeaponDelegate>().SetWeapon(weapon);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
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

    private void Attack()
    {
        attacking = true;
        navigator.enabled = false;
        obstacle.enabled = true;
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length);
    }
}
