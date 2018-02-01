using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void DoBasicAttack(Vector3 targetPosition);

    public abstract void DoSpecialAttack1(Vector3 targetPosition);

    public abstract float GetSpecialAttack1Timer();

    public abstract float GetSpecialAttack1Cooldown();

    public abstract void DoSpecialAttack2(Vector3 targetPosition);

    public abstract float GetSpecialAttack2Timer();

    public abstract float GetSpecialAttack2Cooldown();

    public abstract void AdjustCooldowns(float passedTime);

    public abstract void OnWeaponSwap();
}
