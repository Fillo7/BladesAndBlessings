using UnityEngine;

public class GhostHand : EnemyWeapon
{
    [SerializeField] private GameObject ghostProjectile;

    private float damage = 30.0f;

    private Transform position;
    private Transform target;

    public override void DoAttack()
    {
        Vector3 spellDirection = target.position - position.position;
        GameObject projectileInstance = Instantiate(ghostProjectile, position.position + position.forward * 1.2f + position.up * 1.45f + position.right * 0.6f,
            Quaternion.LookRotation(spellDirection, new Vector3(0.0f, 1.0f, 0.0f))) as GameObject;
        GhostProjectile script = projectileInstance.GetComponent<GhostProjectile>();
        script.SetDamage(damage);
        script.SetOwner(ProjectileOwner.Enemy);
        script.SetDirection(projectileInstance.transform.forward);
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
