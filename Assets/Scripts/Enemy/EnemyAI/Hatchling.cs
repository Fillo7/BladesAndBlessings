using UnityEngine;

public class Hatchling : EnemyAI
{
    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private float damage = 5;
    [SerializeField] private float attackCooldown = 1.5f;

    private float attackRange = 1.75f;
    private float attackTimer = 1.5f;

    protected override void Awake()
    {
        base.Awake();
        navigator.speed = movementSpeed;
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
            TurnTowardsPlayer();

            if (attackTimer > attackCooldown)
            {
                playerHealth.TakeDamage(damage);
                attackTimer = 0.0f;
            }
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
}
