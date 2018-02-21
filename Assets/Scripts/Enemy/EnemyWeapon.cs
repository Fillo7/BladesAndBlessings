using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private Animator animator;
    private EnemyHealth health;

    private float damage = 30.0f;
    private int maxHitCount = 0;

    void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player") || maxHitCount <= 0)
        {
            return;
        }

        maxHitCount--;
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damage);
    }

    public void OnAttackBlock()
    {
        maxHitCount--;
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

    public void SetMaxHitCount(int count)
    {
        maxHitCount = count;
    }
}
