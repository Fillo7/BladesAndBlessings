using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void DoBasicAttack();

    public abstract void DoSpecialAttack1();

    public abstract float GetSpecialAttack1Timer();

    public abstract void DoSpecialAttack2();

    public abstract float GetSpecialAttack2Timer();

    public abstract void AdjustCooldowns(float passedTime);

    public abstract void OnWeaponSwap();

    public abstract WeaponType GetWeaponType();

    public abstract List<AbilityInfo> GetAbilityInfo();

    public abstract AnimatorOverrideController GetAnimatorController();
}
