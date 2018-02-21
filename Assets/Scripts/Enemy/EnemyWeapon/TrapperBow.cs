using UnityEngine;

public class TrapperBow : EnemyWeapon
{
    [SerializeField] private GameObject arrow;

    private float damage = 35.0f;

    private Transform position;
    private Transform target;

    public override void DoAttack()
    {
        Vector3 arrowDirection = target.position - position.position;
        GameObject movingArrow = Instantiate(arrow, position.position + position.forward * 0.7f + position.up * 1.5f,
            Quaternion.LookRotation(arrowDirection, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        DamageProjectile script = movingArrow.GetComponent<DamageProjectile>();
        script.SetDamage(damage);
        script.SetDirection(movingArrow.transform.up);
    }

    public override void DoAlternateAttack()
    {}

    public override void OnAttackBlock()
    {}

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetPosition(Transform position)
    {
        this.position = position;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
