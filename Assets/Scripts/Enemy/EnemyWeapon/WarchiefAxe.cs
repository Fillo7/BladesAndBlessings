using UnityEngine;

public class WarchiefAxe : EnemyWeapon
{
    private Animator animator;
    private OrcWarchief warchiefAI;

    private float damage = 45.0f;
    private int maxHitCount = 0;
    private bool playerHit = false;

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
        playerHit = true;
    }

    public override void DoAttack()
    {
        maxHitCount = 1;
        playerHit = false;
    }

    public override void DoAlternateAttack()
    {}

    public override void OnAttackBlock()
    {
        if (playerHit)
        {
            return;
        }

        maxHitCount = 0;
        animator.SetTrigger("Blocked");
        warchiefAI.CancelInvoke();
        warchiefAI.ResetAttack();
        warchiefAI.ResetBlocking();
        warchiefAI.SetAttackTimer(-0.5f);
    }

    public void Initialize(Animator animator, OrcWarchief warchiefAI, float damage)
    {
        SetEnemyAnimator(animator);
        SetWarchiefAI(warchiefAI);
        SetDamage(damage);
    }

    public void SetEnemyAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public void SetWarchiefAI(OrcWarchief warchiefAI)
    {
        this.warchiefAI = warchiefAI;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
