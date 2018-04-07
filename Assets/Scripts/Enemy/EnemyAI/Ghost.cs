using UnityEngine;

public class Ghost : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float movementSpeed = 4.0f;
    [SerializeField] private float damage = 30.0f;
    [SerializeField] private float attackCooldown = 3.5f;

    private Animator animator;
    private GhostHand weapon;

    private float attackTimer = 2.0f;
    private bool hasDestination = false;
    private bool playerSighted = false;
    private bool attacking = false;
    private bool attacked = false;
    private bool repositioning = false;

    protected override void Awake()
    {
        base.Awake();
        navigator.speed = movementSpeed;
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<GhostHand>();
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

        if (navigator.enabled && navigator.velocity.magnitude > 0.35f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (!playerSighted)
        {
            if (!hasDestination)
            {
                navigator.SetDestination(GetRandomLocation(25.0f));
                hasDestination = true;
            }
            else if (GetDistanceToTarget(navigator.destination) < 1.0f)
            {
                navigator.SetDestination(GetRandomLocation(25.0f));
            }
            
            if (IsPlayerInRange(10.0f))
            {
                playerSighted = true;
            }

            return;
        }

        if (!repositioning)
        {
            attackTimer += Time.deltaTime;
        }
        
        if (attacked && !repositioning)
        {
            navigator.SetDestination(GetRandomLocation(20.0f));
            attacked = false;
            repositioning = true;
        }

        if (repositioning && GetDistanceToTarget(navigator.destination) < 1.0f)
        {
            repositioning = false;
        }

        if (!repositioning || attacking)
        {
            TurnTowardsPlayer(150.0f);
        }

        if (attackTimer > attackCooldown && !attacking && !repositioning)
        {
            PrepareAttack();
        }

        if (!attacking && !IsPlayerInRange(35.0f))
        {
            playerSighted = false;
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
        attackTimer = 0.0f;
        attacking = false;
        attacked = true;
        if (navigator.enabled)
        {
            navigator.isStopped = false;
        }
    }
}
