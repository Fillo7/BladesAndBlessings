using UnityEngine;

public class Aberration : EnemyAI
{
    [SerializeField] private float auraRadius = 4.5f;
    [SerializeField] private float auraDamage = 8.0f;
    [SerializeField] private float tickTime = 0.25f;

    private Animator animator;

    private float tickTimer = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (IsPlayerInRange(auraRadius))
        {
            tickTimer += Time.deltaTime;
            navigator.speed = 0.3f;

            if (tickTimer > tickTime)
            {
                playerHealth.TakeDamage(auraDamage);
                tickTimer = 0.0f;
            }
        }
        else
        {
            tickTimer = 0.0f;
            navigator.speed = movementSpeed;
        }

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

        if (navigator.enabled)
        {
            navigator.SetDestination(player.position);
        }
    }
}
