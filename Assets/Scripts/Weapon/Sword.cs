using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private Animator animator;
    // private WeaponType weaponType = WeaponType.Melee;
    // private PlayerMovementController playerMovement;
    private int baseDamage = 20;

    private int maxHitCount = 0;
    private int damageToDeal = 0;

    private float activeBlockTimer = 0.0f;
    private float activeBlockMax = 5.0f;
    private bool blocking = false;
    private float blockTimer = 0.0f;
    private float blockCooldown = 3.0f;

    private List<GameObject> slashedEnemies = new List<GameObject>();
    private bool slashing = false;
    private float slashTimer = 0.0f;
    private float slashCooldown = 1.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        // playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        blockTimer -= Time.deltaTime;
        slashTimer -= Time.deltaTime;

        if (blocking)
        {
            activeBlockTimer -= Time.deltaTime;
            animator.SetFloat("BlockTimer", activeBlockTimer);

            if (activeBlockTimer <= 0.0f)
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
            enemyHealth.ApplyDot(10.1f, 2.0f, 2);
            slashedEnemies.Add(other.gameObject);
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
        if (blockTimer > 0.0f)
        {
            return;
        }

        animator.SetFloat("BlockTimer", activeBlockMax);
        activeBlockTimer = activeBlockMax;
        blocking = true;
    }

    public override void DoSpecialAttack2(Vector3 targetPosition)
    {
        if (slashTimer > 0.0f)
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
        slashTimer = slashCooldown;
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
        activeBlockTimer = 0.0f;
        animator.SetFloat("BlockTimer", activeBlockTimer);
        blockTimer = blockCooldown;
    }

    private void HandleBlock(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            Arrow arrow = other.gameObject.GetComponent<Arrow>();
            arrow.SwapDirection();
            ResetBlocking();
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
