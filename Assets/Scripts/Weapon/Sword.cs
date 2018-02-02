using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private AnimatorOverrideController animatorController;

    private int baseDamage = 25;

    private int maxHitCount = 0;
    private float damageToDeal = 0;

    private bool blocking = true;
    private float blockTimer = 2.5f;
    private float blockCooldown = 2.5f;

    private List<GameObject> slashedEnemies = new List<GameObject>();
    private bool slashing = false;
    private float slashTimer = 10.0f;
    private float slashCooldown = 10.0f;

    void Update()
    {
        blockTimer += Time.deltaTime;
        slashTimer += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (blocking)
        {
            HandleBlock(other);
            return;
        }

        if (!other.tag.Equals("Enemy") || maxHitCount <= 0 || slashedEnemies.Contains(other.gameObject))
        {
            return;
        }

        maxHitCount--;
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        enemyHealth.TakeDamage(damageToDeal);

        if (slashing)
        {
            slashedEnemies.Add(other.gameObject);
            enemyHealth.ApplyDotEffect(10.1f, 2.0f, 3);
        }
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {
        if (blocking)
        {
            ResetBlocking();
        }

        maxHitCount = 1;
        damageToDeal = (float)baseDamage;
    }

    public override void DoSpecialAttack1(Vector3 targetPosition)
    {
        if (blockTimer < blockCooldown)
        {
            return;
        }

        blocking = true;
        blockTimer = 0.0f;
        Invoke("ResetBlocking", 0.5f);
    }

    public override float GetSpecialAttack1Timer()
    {
        return blockTimer;
    }

    public override void DoSpecialAttack2(Vector3 targetPosition)
    {
        if (slashTimer < slashCooldown)
        {
            return;
        }

        if (blocking)
        {
            ResetBlocking();
        }

        maxHitCount = 5;
        damageToDeal = baseDamage * 1.75f;
        slashing = true;
        slashTimer = 0.0f;

        Invoke("ClearSlash", 2.1f);
    }

    public override float GetSpecialAttack2Timer()
    {
        return slashTimer;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        blockTimer += passedTime;
        slashTimer += passedTime;
    }

    public override void OnWeaponSwap()
    {
    }

    public override List<AbilityInfo> GetAbilityInfo()
    {
        List<AbilityInfo> result = new List<AbilityInfo>();
        result.Add(new AbilityInfo(0.0f, 0.0f, 1.3f));
        result.Add(new AbilityInfo(blockCooldown, 0.0f, 0.5f));
        result.Add(new AbilityInfo(slashCooldown, 0.0f, 2.1f));

        return result;
    }

    public override AnimatorOverrideController GetAnimatorController()
    {
        return animatorController;
    }

    private void ResetBlocking()
    {
        blocking = false;
    }

    private void HandleBlock(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            projectile.ReverseDirection();
            projectile.SetOwner(ProjectileOwner.Player);
        }
        else if (other.tag.Equals("Weapon"))
        {
            EnemyWeapon weapon = other.gameObject.GetComponent<EnemyWeapon>();
            if (weapon != null)
            {
                weapon.OnAttackBlock();
            }
        }
    }

    private void ClearSlash()
    {
        slashing = false;
        slashedEnemies.Clear();
    }
}
