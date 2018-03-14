using UnityEngine;

public class GoblinDagger : EnemyWeapon
{
    private Animator animator;
    private GoblinRogue rogueAI;

    private float damage = 15.0f;
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
        playerHealth.ApplyDotEffect(20.1f, 4.0f, 5.0f);
    }

    public override void DoAttack()
    {
        maxHitCount = 1;
    }

    public override void DoAlternateAttack()
    { }

    public override void OnAttackBlock()
    {
        maxHitCount = 0;
        animator.SetTrigger("Blocked");
        rogueAI.CancelInvoke();
        rogueAI.ResetAttack();
        rogueAI.ApplyStun(2.0f);
    }

    public void Initialize(Animator animator, GoblinRogue rogueAI, float damage)
    {
        SetEnemyAnimator(animator);
        SetRogueAI(rogueAI);
        SetDamage(damage);
    }

    public void SetEnemyAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public void SetRogueAI(GoblinRogue rogueAI)
    {
        this.rogueAI = rogueAI;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
