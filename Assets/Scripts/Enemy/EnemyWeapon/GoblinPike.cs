using UnityEngine;

public class GoblinPike : EnemyWeapon
{
    private Animator animator;
    private EnemyHealth health;

    private float damage = 30.0f;
    private int maxHitCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Weapon"))
        {
            Sword sword = other.GetComponent<Sword>();
            if (sword != null && sword.IsBlocking())
            {
                OnAttackBlock();
                return;
            }
        }

        if (!other.tag.Equals("Player") || maxHitCount <= 0)
        {
            return;
        }

        maxHitCount--;
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damage);
    }

    public override void DoAttack()
    {
        maxHitCount = 1;
    }

    public override void DoAlternateAttack()
    {}

    public override void OnAttackBlock()
    {
        maxHitCount = 0;
        animator.SetTrigger("Blocked");
        health.TakeDamage(damage);
    }

    public void Initialize(Animator animator, EnemyHealth health, float damage)
    {
        SetEnemyAnimator(animator);
        SetEnemyHealth(health);
        SetDamage(damage);
    }

    public void SetEnemyAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public void SetEnemyHealth(EnemyHealth health)
    {
        this.health = health;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
