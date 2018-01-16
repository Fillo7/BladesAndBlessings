using UnityEngine;

public class Sword : Weapon
{
    private int baseDamage = 20;
    /*private WeaponType weaponType = WeaponType.Melee;
    private PlayerMovementController playerMovement;*/
    private Animator animator;

    private int maxHitCount = 0;
    private int damageToDeal = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
        //playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Enemy") || maxHitCount <= 0)
        {
            return;
        }

        maxHitCount--;
        EnemyHealthController enemyHealth = other.GetComponent<EnemyHealthController>();
        enemyHealth.TakeDamage(damageToDeal);
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {
        maxHitCount = 1;
        damageToDeal = baseDamage;
        animator.SetTrigger("BasicAttack");
    }

    public override void DoSpecialAttack1(Vector3 targetPosition)
    {
        // ...
    }

    public override void DoSpecialAttack2(Vector3 targetPosition)
    {
        // ...
    }

    public override float GetOffsetSide()
    {
        return 0.65f;
    }
}
