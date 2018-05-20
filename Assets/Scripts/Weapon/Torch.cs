using System.Collections.Generic;
using UnityEngine;

public class Torch : Weapon
{
    [SerializeField] private AnimatorOverrideController animatorController;
    [SerializeField] private AnimationClip basicAttack;
    [SerializeField] private AnimationClip specialAttack1;
    [SerializeField] private AnimationClip specialAttack2;

    [SerializeField] private GameObject flames;
    [SerializeField] private GameObject fissure;
    [SerializeField] private ParticleSystem flameParticles;

    private PlayerHealth health;
    private PlayerMovement movement;
    private float baseDamage = 10.0f;

    private float fissureTimer = 12.0f;
    private float fissureCooldown = 12.0f;

    private float cleansingFlameTimer = 35.0f;
    private float cleansingFlameCooldown = 35.0f;

    private Vector3 cursorPosition;

    private int floorMask;
    private int mouseTurningMask;

    void Awake()
    {
        cursorPosition = new Vector3(0.0f, 0.0f, 0.0f);
        health = GetComponentInParent<PlayerHealth>();
        movement = GetComponentInParent<PlayerMovement>();

        floorMask = LayerMask.GetMask("Floor");
        mouseTurningMask = LayerMask.GetMask("MouseTurning");
    }

    void Update()
    {
        fissureTimer += Time.deltaTime;
        cleansingFlameTimer += Time.deltaTime;
    }

    public override void DoBasicAttack()
    {
        GameObject spawnedFlames = Instantiate(flames, movement.transform.position + movement.transform.up * 1.9f + movement.transform.forward * -0.2f,
            movement.transform.rotation, movement.transform) as GameObject;
        spawnedFlames.GetComponent<TorchFlames>().SetDamage(baseDamage * 0.4f);
        spawnedFlames.GetComponent<TorchFlames>().SetDuration(basicAttack.length / 1.1f);
    }

    public override void DoSpecialAttack1()
    {
        if (fissureTimer < fissureCooldown)
        {
            return;
        }

        GameObject spawnedFissure = Instantiate(fissure, cursorPosition, Quaternion.identity);
        spawnedFissure.GetComponent<TorchFissure>().SetDamage(baseDamage * 0.7f);
        fissureTimer = 0.0f;
    }

    public override float GetSpecialAttack1Timer()
    {
        return fissureTimer;
    }

    public override void DoSpecialAttack2()
    {
        if (cleansingFlameTimer < cleansingFlameCooldown)
        {
            return;
        }

        flameParticles.Play();
        health.ClearDoTEffects();
        health.TakeDamage(75.0f);
        health.ApplyHoTEffect(new HoTEffect(20.1f, 1.0f, 6.0f));
        cleansingFlameTimer = 0.0f;
    }

    public override float GetSpecialAttack2Timer()
    {
        return cleansingFlameTimer;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        fissureTimer += passedTime;
        cleansingFlameTimer += passedTime;
    }

    public override void OnWeaponSwap()
    {}

    public override void SetCursorPosition(Vector3 position)
    {
        cursorPosition = position;
    }

    public override List<AbilityInfo> GetAbilityInfo()
    {
        List<AbilityInfo> result = new List<AbilityInfo>();
        result.Add(new AbilityInfo(0.0f, basicAttack.length / 0.8f, 0.8f, 0.55f, true, mouseTurningMask));
        result.Add(new AbilityInfo(fissureCooldown, specialAttack1.length / 1.0f, 1.0f, 0.25f, false, floorMask));
        result.Add(new AbilityInfo(cleansingFlameCooldown, specialAttack2.length / 1.0f, 1.0f, 0.25f, false));

        return result;
    }

    public override AnimatorOverrideController GetAnimatorController()
    {
        return animatorController;
    }
}
