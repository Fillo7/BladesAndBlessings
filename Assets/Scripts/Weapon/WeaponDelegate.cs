using UnityEngine;

public class WeaponDelegate : MonoBehaviour
{
    private Weapon weapon = null;

    public void DoBasicAttack()
    {
        weapon.DoBasicAttack();
    }

    public void DoSpecialAttack1()
    {
        weapon.DoSpecialAttack1();
    }

    public void DoSpecialAttack2()
    {
        weapon.DoSpecialAttack2();
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }
}
