using UnityEngine;

public class Sword : Weapon
{
    private Animator animator;
    // private WeaponType weaponType = WeaponType.Melee;
    // private PlayerMovementController playerMovement;
    private int baseDamage = 20;

    private int maxHitCount = 0;
    private int damageToDeal = 0;

    private float maxBlockTime = 5.0f;
    private float blockTimer = 0.0f;
    private bool blocking = false;
    private float specialAttack1Timer = 0.0f;
    private float specialAttack1Cooldown = 10.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        // playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        specialAttack1Timer -= Time.deltaTime;

        if (blocking)
        {
            blockTimer -= Time.deltaTime;
            animator.SetFloat("BlockTimer", blockTimer);

            if (blockTimer <= 0.0f)
            {
                ResetBlocking();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
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
        if (specialAttack1Timer > 0.0f)
        {
            return;
        }

        animator.SetFloat("BlockTimer", maxBlockTime);
        blockTimer = maxBlockTime;
        blocking = true;
        specialAttack1Timer = specialAttack1Cooldown;
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
        blockTimer = 0.0f;
        animator.SetFloat("BlockTimer", blockTimer);
    }
}
