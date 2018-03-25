using UnityEngine;

public class GoblinPike : EnemyWeapon
{
    private Animator animator;
    private GoblinPiker pikerAI;

    private float damage = 30.0f;
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
        pikerAI.CancelInvoke();
        pikerAI.ResetAttack();
        pikerAI.SetAttackTimer(-3.0f);
    }

    public void Initialize(Animator animator, GoblinPiker pikerAI, float damage)
    {
        SetEnemyAnimator(animator);
        SetPikerAI(pikerAI);
        SetDamage(damage);
    }

    public void SetEnemyAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public void SetPikerAI(GoblinPiker pikerAI)
    {
        this.pikerAI = pikerAI;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
