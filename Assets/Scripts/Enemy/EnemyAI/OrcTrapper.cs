using UnityEngine;

public class OrcTrapper : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float damage = 35.0f;
    [SerializeField] private float attackCooldown = 8.0f;
    [SerializeField] private float movementCooldown = 3.0f;

    private Animator animator;
    private TrapperBow weapon;

    private float maximumMovementDistance = 20.0f;
    private float movementTimer = 4.0f;
    private float attackTimer = 2.0f;
    private bool attacking = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<TrapperBow>();
        weapon.SetDamage(damage);
        GetComponentInChildren<EnemyWeaponDelegate>().SetWeapon(weapon);
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            return;
        }

        if (!attacking)
        {
            movementTimer += Time.deltaTime;
        }
        else
        {
            TurnTowardsPlayer(150.0f);
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

        if (movementTimer > movementCooldown && !attacking)
        {
            MoveRandomly();
        }

        if (attackTimer > attackCooldown && IsPlayerInSight() && !attacking)
        {
            PrepareAttack();
        }
    }

    private void MoveRandomly()
    {
        movementTimer = 0.0f;

        if (navigator.enabled)
        {
            navigator.SetDestination(GetRandomLocation(maximumMovementDistance));
        }
    }

    private void PrepareAttack()
    {
        if (navigator.enabled)
        {
            navigator.isStopped = true;
        }
        attacking = true;
        weapon.SetPosition(transform);
        weapon.SetTarget(player);
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length);
    }

    private void ResetAttack()
    {
        attackTimer = Random.Range(0.0f, attackCooldown / 2.0f);
        attacking = false;
        if (navigator.enabled)
        {
            navigator.isStopped = false;
        }
    }
}
