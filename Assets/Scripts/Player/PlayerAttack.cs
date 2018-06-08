using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    [SerializeField] private Slider ability1Slider;
    [SerializeField] private Slider ability2Slider;

    private bool freezeAttack = false;
    private int activeWeaponIndex = 0;
    private List<GameObject> activeWeapons = new List<GameObject>();
    private GameObject currentWeapon = null;
    private Weapon currentWeaponScript = null;
    private List<AbilityInfo> currentAbilityInfo = null;
    private WeaponDelegate weaponDelegate = null;

    private bool attacking = false;
    private float actionTimer = 0.3f;
    private float actionCooldown = 0.3f;
    private float weaponSwapTimer = 0.0f;
    private float movementSpeedMultiplier = 1.0f;
    private float movementSpeed = 5.5f;

    private CustomInputManager inputManager;
    private Animator animator;
    private PlayerHealth health;
    private PlayerMovement movement;
    private float cameraRayLength = 100.0f;

    void Awake()
    {
        weaponDelegate = GetComponentInChildren<WeaponDelegate>();
        inputManager = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponentInChildren<CustomInputManager>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();

        InitializeWeapons();
        SetActiveWeapons(0, 2);
    }
    
    void Update()
    {
        if (health.IsDead())
        {
            return;
        }

        actionTimer += Time.deltaTime;
        weaponSwapTimer += Time.deltaTime;
        ability1Slider.value = currentWeaponScript.GetSpecialAttack1Timer();
        ability2Slider.value = currentWeaponScript.GetSpecialAttack2Timer();

        if (inputManager.GetKeyDown("InputBasicAttack") && TimerIsReady())
        {
            movementSpeedMultiplier = currentAbilityInfo[0].GetAnimationMovementMultiplier();
            EnableAttack();
            currentWeaponScript.SetCursorPosition(GetCursorWorldPosition(currentAbilityInfo[0].GetLayerMask()));
            animator.SetFloat("BasicAbilitySpeedMultiplier", currentAbilityInfo[0].GetAnimationSpeedMultiplier());
            animator.SetTrigger("BasicAbility");
            if (currentAbilityInfo[0].GetMouseTurningFlag())
            {
                movement.SetMouseTurning(true);
            }

            actionTimer = 0.0f;
            Invoke("ResetAttack", currentAbilityInfo[0].GetAnimationDuration());
        }

        if (inputManager.GetKeyDown("InputSpecialAttack1") && TimerIsReady())
        {
            if (currentWeaponScript.GetSpecialAttack1Timer() < currentAbilityInfo[1].GetCooldown())
            {
                return;
            }

            movementSpeedMultiplier = currentAbilityInfo[1].GetAnimationMovementMultiplier();
            EnableAttack();
            currentWeaponScript.SetCursorPosition(GetCursorWorldPosition(currentAbilityInfo[1].GetLayerMask()));
            animator.SetFloat("SpecialAbility1SpeedMultiplier", currentAbilityInfo[1].GetAnimationSpeedMultiplier());
            animator.SetTrigger("SpecialAbility1");
            if (currentAbilityInfo[1].GetMouseTurningFlag())
            {
                movement.SetMouseTurning(true);
            }

            actionTimer = 0.0f;
            Invoke("ResetAttack", currentAbilityInfo[1].GetAnimationDuration());
        }

        if (inputManager.GetKeyDown("InputSpecialAttack2") && TimerIsReady())
        {
            if (currentWeaponScript.GetSpecialAttack2Timer() < currentAbilityInfo[2].GetCooldown())
            {
                return;
            }

            movementSpeedMultiplier = currentAbilityInfo[2].GetAnimationMovementMultiplier();
            EnableAttack();
            currentWeaponScript.SetCursorPosition(GetCursorWorldPosition(currentAbilityInfo[2].GetLayerMask()));
            animator.SetFloat("SpecialAbility2SpeedMultiplier", currentAbilityInfo[2].GetAnimationSpeedMultiplier());
            animator.SetTrigger("SpecialAbility2");
            if (currentAbilityInfo[2].GetMouseTurningFlag())
            {
                movement.SetMouseTurning(true);
            }

            actionTimer = 0.0f;
            Invoke("ResetAttack", currentAbilityInfo[2].GetAnimationDuration());
        }

        if (inputManager.GetKeyDown("InputWeaponSwap") && TimerIsReady())
        {
            SwapWeapon();
            actionTimer = 0.0f;
        }
    }

    public void SetActiveWeapons(int firstIndex, int secondIndex)
    {
        freezeAttack = true;

        activeWeapons.Clear();
        activeWeapons.Add(weapons[firstIndex]);
        activeWeapons.Add(weapons[secondIndex]);
        ActivateWeapon(0);

        freezeAttack = false;
    }

    private void EnableAttack()
    {
        attacking = true;
        movementSpeed = movementSpeedMultiplier * movement.GetSpeed();
        movement.LimitSpeed(movementSpeed);
    }

    public void ResetAttack()
    {
        movement.ResetSpeed(movementSpeed);
        movement.SetMouseTurning(false);

        animator.SetFloat("BasicAbilitySpeedMultiplier", 1.0f);
        animator.SetFloat("SpecialAbility1SpeedMultiplier", 1.0f);
        animator.SetFloat("SpecialAbility2SpeedMultiplier", 1.0f);
        
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
        if (currentWeapon != null)
        {
            currentWeaponScript.OnWeaponSwap();
            currentWeapon.SetActive(false);
            currentWeaponScript = null;
        }

        activeWeaponIndex = weaponIndex;
        currentWeapon = activeWeapons[activeWeaponIndex];
        currentWeapon.SetActive(true);
        currentWeaponScript = currentWeapon.GetComponentInChildren<Weapon>();
        currentWeaponScript.AdjustCooldowns(weaponSwapTimer);
        currentAbilityInfo = currentWeaponScript.GetAbilityInfo();
        weaponSwapTimer = 0.0f;
        animator.runtimeAnimatorController = currentWeaponScript.GetAnimatorController();
        animator.Rebind();
        InitializeCooldownSliders();
        weaponDelegate.SetWeapon(currentWeaponScript);

        freezeAttack = false;
    }

    private void SwapWeapon()
    {
        int newWeaponIndex = (activeWeaponIndex + 1) % 2;
        ActivateWeapon(newWeaponIndex);
    }

    private void InitializeCooldownSliders()
    {
        ability1Slider.maxValue = currentAbilityInfo[1].GetCooldown();
        ability2Slider.maxValue = currentAbilityInfo[2].GetCooldown();
        ability1Slider.value = currentWeaponScript.GetSpecialAttack1Timer();
        ability2Slider.value = currentWeaponScript.GetSpecialAttack2Timer();
    }

    private Vector3 GetCursorWorldPosition(int layerMask)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, layerMask))
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
