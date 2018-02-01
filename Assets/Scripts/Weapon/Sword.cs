using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private int baseDamage = 25;

    private int maxHitCount = 0;
    private float damageToDeal = 0;

    private float activeBlockTimer = 3.1f;
    private float activeBlockMax = 3.0f;
    private bool blocking = false;
    private float blockTimer = 5.0f;
    private float blockCooldown = 5.0f;

    private List<GameObject> slashedEnemies = new List<GameObject>();
    private bool slashing = false;
    private float slashTimer = 10.0f;
    private float slashCooldown = 10.0f;

    void Update()
    {
        blockTimer += Time.deltaTime;
        slashTimer += Time.deltaTime;

        if (blocking)
        {
            activeBlockTimer += Time.deltaTime;

            if (activeBlockTimer > activeBlockMax)
            {
                ResetBlocking();
            }
        }
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

        activeBlockTimer = 0.0f;
        blocking = true;
    }

    public override float GetSpecialAttack1Timer()
    {
        return blockTimer;
    }

    public override float GetSpecialAttack1Cooldown()
    {
        return blockCooldown;
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
        slashTimer = 0.0f;
    }

    public override float GetSpecialAttack2Timer()
    {
        return slashTimer;
    }

    public override float GetSpecialAttack2Cooldown()
    {
        return slashCooldown;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        blockTimer += passedTime;
        slashTimer += passedTime;
    }

    public override void OnWeaponSwap()
    {
        if (blocking)
        {
            ResetBlocking();
        }
    }

    private void ResetBlocking()
    {
        blocking = false;
        activeBlockTimer = activeBlockMax + 0.1f;
        blockTimer = 0.0f;
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

    private void SetSlash()
    {
        slashing = true;
    }

    private void ClearSlash()
    {
        slashing = false;
        slashedEnemies.Clear();
    }
}
