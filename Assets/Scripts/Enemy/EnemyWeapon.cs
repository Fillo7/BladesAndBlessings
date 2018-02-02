using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private EnemyHealth health;
    [SerializeField] private int damage = 30;

    private bool attackBlocked = false;
    private float attackBlockedTimer = 0.0f;
    private float attackBlockedThreshold = 1.5f;

    void Update()
    {
        if (attackBlocked)
        {
            attackBlockedTimer += Time.deltaTime;

            if (attackBlockedTimer > attackBlockedThreshold)
            {
                attackBlocked = false;
                attackBlockedTimer = 0.0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player") || attackBlocked)
        {
            return;
        }

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damage);
    }

    public void OnAttackBlock()
    {
        attackBlocked = true;
        enemyAnimator.SetTrigger("Blocked");
        health.TakeDamage(damage * 1.5f);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
