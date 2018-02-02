using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    [SerializeField] private Slider ability1Slider;
    [SerializeField] private Slider ability2Slider;
    private GameObject activeWeapon = null;
    private Weapon activeWeaponScript = null;
    private int activeWeaponIndex = 0;

    private float weaponSwapTimer = 0.0f;

    private bool freezeAttack = false;
    private float timeBetweenAttacks = 0.3f;
    private float timer = 0.0f;

    private Animator animator;
    private PlayerMovement playerMovement;
    private int floorMask;
    private float cameraRayLength = 100.0f;

    private AttackCommand attackCommand = AttackCommand.Basic;
    private Vector3 attackTarget = Vector3.zero;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        floorMask = LayerMask.GetMask("Floor");

        InitializeWeapons();
        ActivateWeapon(activeWeaponIndex);
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        weaponSwapTimer += Time.deltaTime;
        ability1Slider.value = activeWeaponScript.GetSpecialAttack1Timer();
        ability2Slider.value = activeWeaponScript.GetSpecialAttack2Timer();

        if (Input.GetButton("Fire1") && TimerIsReady())
        {
            playerMovement.EnableMovement(false);

            animator.SetTrigger("BasicAbility");
            attackCommand = AttackCommand.Basic;
            attackTarget = GetCursorWorldPosition();
            ResetTimer();

            Invoke("FinishAttack", 1.0f);
        }

        if (Input.GetButton("Fire2") && TimerIsReady())
        {
            playerMovement.EnableMovement(false);

            animator.SetTrigger("SpecialAbility1");
            attackCommand = AttackCommand.Special1;
            attackTarget = GetCursorWorldPosition();
            ResetTimer();

            Invoke("FinishAttack", 1.0f);
        }

        if (Input.GetButton("Fire3") && TimerIsReady())
        {
            playerMovement.EnableMovement(false);

            animator.SetTrigger("SpecialAbility2");
            attackCommand = AttackCommand.Special2;
            attackTarget = GetCursorWorldPosition();
            ResetTimer();

            Invoke("FinishAttack", 1.0f);
        }

        if (Input.GetButton("SwapWeapon") && TimerIsReady())
        {
            SwapWeapon();
            ResetTimer();
        }
    }

    public void Attack(AttackCommand command, Vector3 target)
    {
        if (command == AttackCommand.Basic)
        {
            activeWeaponScript.DoBasicAttack(target);
        }
        else if (command == AttackCommand.Special1)
        {
            activeWeaponScript.DoSpecialAttack1(target);
        }
        else if (command == AttackCommand.Special2)
        {
            activeWeaponScript.DoSpecialAttack2(target);
        }
    }

    private void FinishAttack()
    {
        Attack(attackCommand, attackTarget);
        playerMovement.EnableMovement(true);
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
        weaponSwapTimer = 0.0f;
        animator.runtimeAnimatorController = activeWeaponScript.GetAnimatorController();
        InitializeCooldownSliders();
        freezeAttack = false;
    }

    private void SwapWeapon()
    {
        activeWeaponIndex = (activeWeaponIndex + 1) % 2;
        ActivateWeapon(activeWeaponIndex);
    }

    private void InitializeCooldownSliders()
    {
        ability1Slider.maxValue = activeWeaponScript.GetSpecialAttack1Cooldown();
        ability2Slider.maxValue = activeWeaponScript.GetSpecialAttack2Cooldown();
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
        return timer >= timeBetweenAttacks && Time.timeScale != 0.0f && !freezeAttack;
    }

    private void ResetTimer()
    {
        timer = 0.0f;
    }
}
