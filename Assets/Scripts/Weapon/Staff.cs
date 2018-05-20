using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] private AnimatorOverrideController animatorController;
    [SerializeField] private AnimationClip basicAttack;
    [SerializeField] private AnimationClip specialAttack1;
    [SerializeField] private AnimationClip specialAttack2;

    [SerializeField] private GameObject magicBolt;
    [SerializeField] private GameObject staffBlizzard;

    private PlayerMovement movement;

    private float novaTimer = 40.0f;
    private float novaCooldown = 40.0f;

    private float swiftnessTimer = 25.0f;
    private float swiftnessCooldown = 25.0f;

    private int mouseTurningMask;

    void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
        mouseTurningMask = LayerMask.GetMask("MouseTurning");
    }

    void Update()
    {
        novaTimer += Time.deltaTime;
        swiftnessTimer += Time.deltaTime;
    }

    public override void DoBasicAttack()
    {
        GameObject magicBoltInstance = Instantiate(magicBolt, movement.transform.position + movement.transform.right * 0.5f + movement.transform.forward * 1.5f + movement.transform.up * 1.1f,
            Quaternion.LookRotation(movement.transform.forward, new Vector3(0.0f, 1.0f, 0.0f))) as GameObject;
        DamageProjectile script = magicBoltInstance.GetComponent<DamageProjectile>();
        script.SetDirection(movement.transform.forward);
    }

    public override void DoSpecialAttack1()
    {
        if (novaTimer < novaCooldown)
        {
            return;
        }

        Instantiate(staffBlizzard, new Vector3(movement.transform.position.x, movement.transform.position.y + 1.0f, movement.transform.position.z), Quaternion.identity);
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

        movement.ApplyMovementEffect(new MovementEffect(10.0f, 1.75f));
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
        result.Add(new AbilityInfo(0.0f, basicAttack.length / 0.75f, 0.75f, 0.3f, true, mouseTurningMask));
        result.Add(new AbilityInfo(novaCooldown, specialAttack1.length / 0.6f, 0.6f, 0.1f, false));
        result.Add(new AbilityInfo(swiftnessCooldown, specialAttack2.length, 1.0f, 0.6f, false));

        return result;
    }

    public override AnimatorOverrideController GetAnimatorController()
    {
        return animatorController;
    }
};
