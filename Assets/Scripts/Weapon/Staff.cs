using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] private AnimatorOverrideController animatorController;
    [SerializeField] private AnimationClip basicAttack;
    [SerializeField] private AnimationClip specialAttack1;
    [SerializeField] private AnimationClip specialAttack2;

    [SerializeField] private GameObject magicBolt;

    private PlayerMovement movement;

    private float novaTimer = 20.0f;
    private float novaCooldown = 20.0f;

    private float swiftnessTimer = 25.0f;
    private float swiftnessCooldown = 25.0f;

    void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        novaTimer += Time.deltaTime;
        swiftnessTimer += Time.deltaTime;
    }

    public override void DoBasicAttack()
    {
        GameObject magicBoltInstance = Instantiate(magicBolt, movement.transform.position + movement.transform.forward * 0.7f + movement.transform.up * 1.35f,
            Quaternion.LookRotation(movement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        DamageProjectile script = magicBoltInstance.GetComponent<DamageProjectile>();
        script.SetDirection(movement.transform.forward);
    }

    public override void DoSpecialAttack1()
    {
        if (novaTimer < novaCooldown)
        {
            return;
        }

        // todo
        novaTimer = 0.0f;
    }

    public override float GetSpecialAttack1Timer()
    {
        return novaTimer;
    }

    public override void DoSpecialAttack2()
    {
        if (swiftnessTimer < swiftnessCooldown)
        {
            return;
        }

        movement.ApplyMovementEffect(new MovementEffect(8.0f, 2.0f));
        swiftnessTimer = 0.0f;
    }

    public override float GetSpecialAttack2Timer()
    {
        return swiftnessTimer;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        novaTimer += passedTime;
        swiftnessTimer += passedTime;
    }

    public override void OnWeaponSwap()
    {}

    public override void SetCursorPosition(Vector3 position)
    {}

    public override List<AbilityInfo> GetAbilityInfo()
    {
        List<AbilityInfo> result = new List<AbilityInfo>();
        result.Add(new AbilityInfo(0.0f, basicAttack.length / 1.0f, 1.0f, 1.0f, true));
        result.Add(new AbilityInfo(novaCooldown, specialAttack1.length / 1.0f, 1.0f, 1.0f, false));
        result.Add(new AbilityInfo(swiftnessCooldown, specialAttack2.length / 1.0f, 1.0f, 1.0f, false));

        return result;
    }

    public override AnimatorOverrideController GetAnimatorController()
    {
        return animatorController;
    }
};
