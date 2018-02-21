using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    public abstract void DoAttack();

    public abstract void DoAlternateAttack();

    public abstract void OnAttackBlock();
}
