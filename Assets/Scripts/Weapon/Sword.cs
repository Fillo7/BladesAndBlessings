using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private AnimatorOverrideController animatorController;
    [SerializeField] private AnimationClip basicAttack;
    [SerializeField] private AnimationClip specialAttack1;
    [SerializeField] private AnimationClip specialAttack2;

    private float baseDamage = 25.0f;

    private int maxHitCount = 0;
    private float damageToDeal = 0;

    private List<GameObject> blockedObjects = new List<GameObject>();
    private bool blocking = true;
    private float blockTimer = 3.0f;
    private float blockCooldown = 3.0f;

    private List<GameObject> slashedEnemies = new List<GameObject>();
    private bool coneSlashing = false;
    private float coneSlashTimer = 16.0f;
    private float coneSlashCooldown = 16.0f;

    void Update()
    {
        blockTimer += Time.deltaTime;
        coneSlashTimer += Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (blocking)
        {
            if (!blockedObjects.Contains(other.gameObject))
            {
                HandleBlock(other);
            }
            
            return;
        }

        if (!other.tag.Equals("Enemy") || maxHitCount <= 0 || slashedEnemies.Contains(other.gameObject))
        {
            return;
        }

        maxHitCount--;
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        enemyHealth.TakeDamage(damageToDeal, DamageType.Slashing);

        if (coneSlashing)
        {
            slashedEnemies.Add(other.gameObject);
            enemyHealth.ApplyDoTEffect(new DoTEffect(10.1f, 2.0f, 6.0f, DamageType.Bleeding));
        }
    }

    public override void DoBasicAttack()
    {
        if (blocking)
        {
            ResetBlock();
        }

        maxHitCount = 1;
        damageToDeal = baseDamage;
    }

    public override void DoSpecialAttack1()
    {
        if (blockTimer < blockCooldown)
        {
            return;
        }

        blocking = true;
        blockTimer = 0.0f;
        Invoke("ResetBlock", specialAttack1.length);
    }

    public override float GetSpecialAttack1Timer()
    {
        return blockTimer;
    }

    public override void DoSpecialAttack2()
    {
        if (coneSlashTimer < coneSlashCooldown)
        {
            return;
        }

        if (blocking)
        {
            ResetBlock();
        }

        maxHitCount = 3;
        damageToDeal = baseDamage * 1.3f;
        coneSlashing = true;
        coneSlashTimer = 0.0f;

        Invoke("ResetConeSlash", specialAttack2.length);
    }

    public override float GetSpecialAttack2Timer()
    {
        return coneSlashTimer;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        blockTimer += passedTime;
        coneSlashTimer += passedTime;
    }

    public override void OnWeaponSwap()
    {
        if (blocking)
        {
            ResetBlock();
        }
    }

    public override void SetCursorPosition(Vector3 position)
    {}

    public override List<AbilityInfo> GetAbilityInfo()
    {
        List<AbilityInfo> result = new List<AbilityInfo>();
        result.Add(new AbilityInfo(0.0f, basicAttack.length / 1.2f, 1.2f, 0.25f, false));
        result.Add(new AbilityInfo(blockCooldown, specialAttack1.length / 1.15f, 1.15f, 0.6f, false));
        result.Add(new AbilityInfo(coneSlashCooldown, specialAttack2.length / 1.15f, 1.15f, 0.25f, false));

        return result;
    }

    public override AnimatorOverrideController GetAnimatorController()
    {
        return animatorController;
    }

    private void HandleBlock(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            projectile.ReverseDirection();
            projectile.SetOwner(ProjectileOwner.Player);
        }

        blockedObjects.Add(other.gameObject);
    }

    public bool IsBlocking()
    {
        return blocking;
    }

    private void ResetBlock()
    {
        blocking = false;
        blockedObjects.Clear();
    }

    private void ResetConeSlash()
    {
        coneSlashing = false;
        slashedEnemies.Clear();
    }
}
