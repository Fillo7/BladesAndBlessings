using UnityEngine;

public class Bow : Weapon
{
    private int baseDamage = 15;
    private WeaponType weaponType = WeaponType.Ranged;
    private PlayerMovementController playerMovement;

    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {
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

    public override float GetOffsetPosition()
    {
        return 0.65f;
    }
}
