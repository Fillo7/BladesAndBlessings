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

    void Awake()
    {
        animator = GetComponent<Animator>();
        // playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        blockTimer -= Time.deltaTime;

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

        if (!other.tag.Equals("Enemy") || maxHitCount <= 0)
        {
            return;
        }

        maxHitCount--;
        EnemyHealthController enemyHealth = other.GetComponent<EnemyHealthController>();
        enemyHealth.TakeDamage(damageToDeal);
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
        // ...
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
}
