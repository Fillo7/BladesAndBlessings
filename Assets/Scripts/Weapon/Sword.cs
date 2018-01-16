using UnityEngine;

public class Sword : Weapon
{
    private int baseDamage = 20;
    private WeaponType weaponType = WeaponType.Melee;
    private PlayerMovementController playerMovement;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {
        animator.SetTrigger("BasicAttack");
        // ...
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
