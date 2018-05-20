using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private AnimatorOverrideController animatorController;
    [SerializeField] private AnimationClip basicAttack;
    [SerializeField] private AnimationClip specialAttack1;
    [SerializeField] private AnimationClip specialAttack2;

    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject chargedArrow;

    private PlayerMovement playerMovement;
    private float baseDamage = 15.0f;
    private float arrowSpeed = 20.0f;

    private float arrowFanTimer = 8.0f;
    private float arrowFanCooldown = 8.0f;

    private float chargedArrowTimer = 15.0f;
    private float chargedArrowCooldown = 15.0f;

    private int mouseTurningMask;

    void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        mouseTurningMask = LayerMask.GetMask("MouseTurning");
    }

    void Update()
    {
        arrowFanTimer += Time.deltaTime;
        chargedArrowTimer += Time.deltaTime;
    }

    public override void DoBasicAttack()
    {
        SpawnArrow();
    }

    public override void DoSpecialAttack1()
    {
        if (arrowFanTimer < arrowFanCooldown)
        {
            return;
        }

        SpawnArrowFan();
        arrowFanTimer = 0.0f;
    }

    public override float GetSpecialAttack1Timer()
    {
        return arrowFanTimer;
    }

    public override void DoSpecialAttack2()
    {
        if (chargedArrowTimer < chargedArrowCooldown)
        {
            return;
        }

        SpawnChargedArrow();
        chargedArrowTimer = 0.0f;
    }

    public override float GetSpecialAttack2Timer()
    {
        return chargedArrowTimer;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        arrowFanTimer += passedTime;
        chargedArrowTimer += passedTime;
    }

    public override void OnWeaponSwap()
    {}

    public override void SetCursorPosition(Vector3 position)
    {}

    public override List<AbilityInfo> GetAbilityInfo()
    {
        List<AbilityInfo> result = new List<AbilityInfo>();
        result.Add(new AbilityInfo(0.0f, basicAttack.length / 1.1f, 1.1f, 0.35f, true, mouseTurningMask));
        result.Add(new AbilityInfo(arrowFanCooldown, specialAttack1.length / 1.15f, 1.15f, 0.2f, true, mouseTurningMask));
        result.Add(new AbilityInfo(chargedArrowCooldown, specialAttack2.length / 1.15f, 1.15f, 0.2f, true, mouseTurningMask));

        return result;
    }

    public override AnimatorOverrideController GetAnimatorController()
    {
        return animatorController;
    }

    public void SpawnArrow()
    {
        GameObject movingArrow = Instantiate(arrow, playerMovement.transform.position + playerMovement.transform.forward * 0.7f + playerMovement.transform.up * 1.35f,
            Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        DamageProjectile script = movingArrow.GetComponent<DamageProjectile>();
        script.SetDamage(baseDamage);
        script.SetSpeed(arrowSpeed);
        script.SetDirection(playerMovement.transform.forward);
    }

    public void SpawnArrowFan()
    {
        for (int i = -2; i < 3; i++)
        {
            GameObject movingArrow = Instantiate(arrow, playerMovement.transform.position + playerMovement.transform.forward * 0.7f + playerMovement.transform.up * 1.35f,
                Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f) * Quaternion.Euler(i * 25.0f, 0.0f, 0.0f)) as GameObject;

            if (playerMovement.transform.forward == Vector3.right || playerMovement.transform.forward == Vector3.left)
            {
                movingArrow.transform.rotation = Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f) * Quaternion.Euler(0.0f, 0.0f, i * 25.0f);
            }

            DamageProjectile script = movingArrow.GetComponent<DamageProjectile>();
            script.SetDamage(baseDamage);
            script.SetSpeed(arrowSpeed);
            script.SetDirection(movingArrow.transform.up);
        }
    }

    public void SpawnChargedArrow()
    {
        GameObject movingArrow = Instantiate(chargedArrow, playerMovement.transform.position + playerMovement.transform.forward * 0.7f + playerMovement.transform.up * 1.35f,
            Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        ArrowCharged script = movingArrow.GetComponent<ArrowCharged>();
        script.SetDamage(baseDamage * 2.0f);
        script.SetSpeed(arrowSpeed * 2.0f);
        script.SetDirection(playerMovement.transform.forward);
    }
}
