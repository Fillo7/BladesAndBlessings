using System.Collections;
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

    private PlayerMovement playerMovement;
    private float baseDamage = 10.0f;

    private float fissureTimer = 10.0f;
    private float fissureCooldown = 10.0f;

    private float flameCloakTimer = 25.0f;
    private float flameCloakCooldown = 25.0f;

    private Vector3 cursorPosition;

    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        cursorPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void Update()
    {
        fissureTimer += Time.deltaTime;
        flameCloakTimer += Time.deltaTime;
    }

    public override void DoBasicAttack()
    {
        GameObject spawnedFlames = Instantiate(flames, gameObject.transform);
        spawnedFlames.GetComponent<TorchFlames>().SetDamage(baseDamage);
    }

    public override void DoSpecialAttack1()
    {
        if (fissureTimer < fissureCooldown)
        {
            return;
        }

        GameObject spawnedFissure = Instantiate(fissure, cursorPosition, Quaternion.identity);
        spawnedFissure.GetComponent<TorchFissure>().SetDamage(baseDamage * 0.8f);
        fissureTimer = 0.0f;
    }

    public override float GetSpecialAttack1Timer()
    {
        return fissureTimer;
    }

    public override void DoSpecialAttack2()
    {
        if (flameCloakTimer < flameCloakCooldown)
        {
            return;
        }

        // todo
        flameCloakTimer = 0.0f;
    }

    public override float GetSpecialAttack2Timer()
    {
        return flameCloakTimer;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        fissureTimer += passedTime;
        flameCloakTimer += passedTime;
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
        result.Add(new AbilityInfo(0.0f, basicAttack.length / 1.0f, 1.0f, false));
        result.Add(new AbilityInfo(fissureCooldown, specialAttack1.length / 1.0f, 1.0f, true));
        result.Add(new AbilityInfo(flameCloakCooldown, specialAttack2.length / 1.0f, 1.0f, false));

        return result;
    }

    public override AnimatorOverrideController GetAnimatorController()
    {
        return animatorController;
    }
}
