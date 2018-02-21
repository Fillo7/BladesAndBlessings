using UnityEngine;

public class EnemyWeaponDelegate : MonoBehaviour
{
    private EnemyWeapon weapon = null;

    public void DoAttack()
    {
        weapon.DoAttack();
    }

    public void DoAlternateAttack()
    {
        weapon.DoAlternateAttack();
    }

    public void SetWeapon(EnemyWeapon weapon)
    {
        this.weapon = weapon;
    }
}
