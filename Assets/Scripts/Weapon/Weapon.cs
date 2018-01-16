using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void DoBasicAttack(Vector3 targetPosition);

    public abstract void DoSpecialAttack1(Vector3 targetPosition);

    public abstract void DoSpecialAttack2(Vector3 targetPosition);

    public abstract float GetOffsetPosition();
}
