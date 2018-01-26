﻿using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private Animator animator;
    // private WeaponType weaponType = WeaponType.Melee;
    // private PlayerMovementController playerMovement;
    private int baseDamage = 20;

    private int maxHitCount = 0;
    private int damageToDeal = 0;

    private float activeBlockTimer = 3.1f;
    private float activeBlockMax = 3.0f;
    private bool blocking = false;
    private float blockTimer = 5.0f;
    private float blockCooldown = 5.0f;

    private List<GameObject> slashedEnemies = new List<GameObject>();
    private bool slashing = false;
    private float slashTimer = 10.0f;
    private float slashCooldown = 10.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        // playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        blockTimer += Time.deltaTime;
        slashTimer += Time.deltaTime;

        if (blocking)
        {
            activeBlockTimer += Time.deltaTime;
            animator.SetFloat("BlockTimer", activeBlockTimer);

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
            enemyHealth.ApplyDot(10.1f, 2.0f, 2);
        }
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {
        if (blocking)
        {
            ResetBlocking();
        }

        maxHitCount = 1;
        damageToDeal = baseDamage;
        animator.SetTrigger("BasicAttack");
    }

    public override void DoSpecialAttack1(Vector3 targetPosition)
    {
        if (blockTimer < blockCooldown)
        {
            return;
        }

        animator.SetFloat("BlockTimer", 0.0f);
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
        damageToDeal = baseDamage * 2;
        animator.SetTrigger("SwordSlash");
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

    public override float GetOffsetSide()
    {
        return 0.65f;
    }

    public override float GetOffsetHeight()
    {
        return -1.0f;
    }

    private void ResetBlocking()
    {
        blocking = false;
        activeBlockTimer = activeBlockMax + 0.1f;
        animator.SetFloat("BlockTimer", activeBlockMax + 0.1f);
        blockTimer = 0.0f;
    }

    private void HandleBlock(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            Arrow arrow = other.gameObject.GetComponent<Arrow>();
            arrow.SwapDirection();
            arrow.SetOwner(ProjectileOwner.Player);
        }

        // to do: handle melee attacks
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
