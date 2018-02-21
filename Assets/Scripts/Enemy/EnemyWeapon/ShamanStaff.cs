using UnityEngine;

public class ShamanStaff : EnemyWeapon
{
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject healingProjectile;

    private float damage = 50.0f;
    private float healing = 30.0f;

    private Transform position;
    private Transform target;

    public override void DoAttack()
    {
        Vector3 spellDirection = target.position - position.position;
        GameObject fireballInstance = Instantiate(fireball, position.position + position.forward * 2.0f + position.up * 1.1f,
            Quaternion.LookRotation(spellDirection, new Vector3(0.0f, 1.0f, 0.0f))) as GameObject;
        DamageProjectile script = fireballInstance.GetComponent<DamageProjectile>();
        script.SetDamage(damage);
        script.SetOwner(ProjectileOwner.Enemy);
        script.SetDirection(fireballInstance.transform.forward);
    }

    public override void DoAlternateAttack()
    {
        Vector3 spellDirection = target.position - position.position;
        GameObject healingProjectileInstance = Instantiate(healingProjectile, position.position + position.forward * 2.0f + position.up * 1.1f,
            Quaternion.LookRotation(spellDirection, new Vector3(0.0f, 1.0f, 0.0f))) as GameObject;
        HealProjectile script = healingProjectileInstance.GetComponent<HealProjectile>();
        script.SetHealing(healing);
        script.SetOwner(ProjectileOwner.Enemy);
        script.SetDirection(healingProjectileInstance.transform.forward);
    }

    public override void OnAttackBlock()
    {}

    public void Initialize(float damage, float healing)
    {
        SetDamage(damage);
        SetHealing(healing);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetHealing(float healing)
    {
        this.healing = healing;
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
