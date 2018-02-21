﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    [SerializeField] private Slider ability1Slider;
    [SerializeField] private Slider ability2Slider;

    private bool freezeAttack = false;
    private int activeWeaponIndex = 0;
    private GameObject activeWeapon = null;
    private Weapon activeWeaponScript = null;
    private List<AbilityInfo> activeAbilityInfo = null;
    private WeaponDelegate weaponDelegate = null;

    private bool attacking = false;
    private float actionTimer = 0.3f;
    private float actionCooldown = 0.3f;
    private float weaponSwapTimer = 0.0f;

    private Animator animator;
    private PlayerHealth health;
    private PlayerMovement movement;
    private int floorMask;
    private float cameraRayLength = 100.0f;

    void Awake()
    {
        weaponDelegate = GetComponentInChildren<WeaponDelegate>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();
        floorMask = LayerMask.GetMask("Floor");

        InitializeWeapons();
        ActivateWeapon(activeWeaponIndex);
    }
    
    void Update()
    {
        if (health.IsDead())
        {
            return;
        }

        actionTimer += Time.deltaTime;
        weaponSwapTimer += Time.deltaTime;
        ability1Slider.value = activeWeaponScript.GetSpecialAttack1Timer();
        ability2Slider.value = activeWeaponScript.GetSpecialAttack2Timer();

        if (Input.GetButton("Fire1") && TimerIsReady())
        {
            EnableAttack();
            animator.SetTrigger("BasicAbility");

            if (activeWeaponScript.GetWeaponType() == WeaponType.Ranged)
            {
                movement.TurnTowardsDirection(GetCursorWorldPosition(), activeAbilityInfo[0].GetAnimationDuration());
            }

            actionTimer = 0.0f;
            Invoke("ResetAttack", activeAbilityInfo[0].GetAnimationDuration());
        }

        if (Input.GetButton("Fire2") && TimerIsReady())
        {
            if (activeWeaponScript.GetSpecialAttack1Timer() < activeAbilityInfo[1].GetCooldown())
            {
                return;
            }

            EnableAttack();
            animator.SetTrigger("SpecialAbility1");

            if (activeWeaponScript.GetWeaponType() == WeaponType.Ranged)
            {
                movement.TurnTowardsDirection(GetCursorWorldPosition(), activeAbilityInfo[1].GetAnimationDuration());
            }

            actionTimer = 0.0f;
            Invoke("ResetAttack", activeAbilityInfo[1].GetAnimationDuration());
        }

        if (Input.GetButton("Fire3") && TimerIsReady())
        {
            if (activeWeaponScript.GetSpecialAttack2Timer() < activeAbilityInfo[2].GetCooldown())
            {
                return;
            }

            EnableAttack();
            animator.SetTrigger("SpecialAbility2");

            if (activeWeaponScript.GetWeaponType() == WeaponType.Ranged)
            {
                movement.TurnTowardsDirection(GetCursorWorldPosition(), activeAbilityInfo[2].GetAnimationDuration());
            }

            actionTimer = 0.0f;
            Invoke("ResetAttack", activeAbilityInfo[2].GetAnimationDuration());
        }

        if (Input.GetButton("SwapWeapon") && TimerIsReady())
        {
            SwapWeapon();
            actionTimer = 0.0f;
        }
    }

    private void EnableAttack()
    {
        attacking = true;
        movement.EnableMovement(false);
    }

    private void ResetAttack()
    {
        movement.EnableMovement(true);
        attacking = false;
    }

    private void InitializeWeapons()
    {
        freezeAttack = true;

        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }

        freezeAttack = false;
    }

    private void ActivateWeapon(int weaponIndex)
    {
        freezeAttack = true;
        if (activeWeapon != null)
        {
            activeWeaponScript.OnWeaponSwap();
            activeWeapon.SetActive(false);
            activeWeaponScript = null;
        }

        activeWeapon = weapons[activeWeaponIndex];
        activeWeapon.SetActive(true);
        activeWeaponScript = activeWeapon.GetComponentInChildren<Weapon>();
        activeWeaponScript.AdjustCooldowns(weaponSwapTimer);
        activeAbilityInfo = activeWeaponScript.GetAbilityInfo();
        weaponSwapTimer = 0.0f;
        animator.runtimeAnimatorController = activeWeaponScript.GetAnimatorController();
        animator.Rebind();
        InitializeCooldownSliders();
        weaponDelegate.SetWeapon(activeWeaponScript);

        freezeAttack = false;
    }

    private void SwapWeapon()
    {
        activeWeaponIndex = (activeWeaponIndex + 1) % 2;
        ActivateWeapon(activeWeaponIndex);
    }

    private void InitializeCooldownSliders()
    {
        ability1Slider.maxValue = activeAbilityInfo[1].GetCooldown();
        ability2Slider.maxValue = activeAbilityInfo[2].GetCooldown();
        ability1Slider.value = activeWeaponScript.GetSpecialAttack1Timer();
        ability2Slider.value = activeWeaponScript.GetSpecialAttack2Timer();
    }

    private Vector3 GetCursorWorldPosition()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            return floorHit.point;
        }
        return new Vector3();
    }

    private bool TimerIsReady()
    {
        return Time.timeScale != 0.0f && actionTimer >= actionCooldown && !freezeAttack && !attacking;
    }
}
