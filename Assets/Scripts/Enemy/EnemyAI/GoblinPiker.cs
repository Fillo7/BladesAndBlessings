using UnityEngine;

public class GoblinPiker : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float movementSpeed = 4.5f;
    [SerializeField] private float damage = 30.0f;
    [SerializeField] private float attackCooldown = 2.0f;

    private Animator animator;
    private GoblinPike weapon;

    private float attackRange = 1.8f;
    private float attackTimer = 2.0f;
    private bool attacking = false;

    protected override void Awake()
    {
        base.Awake();
        navigator.speed = movementSpeed;
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<GoblinPike>();
        weapon.Initialize(animator, enemyHealth, damage);
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
                navigator.enabled = true;
            }
        }

        if (navigator.enabled)
        {
            navigator.SetDestination(player.position);
        }
    }

    private void Attack()
    {
        navigator.enabled = false;
        attacking = true;
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length);
    }

    private void ResetAttack()
    {
        attackTimer = 0.0f;
        attacking = false;
        navigator.enabled = true;
    }
}
