using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponPrefabs = new List<GameObject>();
    private List<GameObject> weapons = new List<GameObject>();
    private GameObject activeWeapon = null;
    private Weapon activeWeaponScript = null;
    private int activeWeaponIndex = 0;

    private bool freezeAttack = false;
    private float timeBetweenAttacks = 0.3f;
    private float timer = 0.0f;

    private PlayerMovementController playerMovement;
    private int floorMask;
    private float cameraRayLength = 100.0f;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovementController>();
        floorMask = LayerMask.GetMask("Floor");

        InitializeWeapons();
        ActivateWeapon(activeWeaponIndex);
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetButton("Fire1") && TimerIsReady())
        {
            Attack(AttackCommand.Basic, GetCursorWorldPosition());
            ResetTimer();
        }

        if (Input.GetButton("Fire2") && TimerIsReady())
        {
            Attack(AttackCommand.Special1, GetCursorWorldPosition());
            ResetTimer();
        }

        if (Input.GetButton("Fire3") && TimerIsReady())
        {
            Attack(AttackCommand.Special2, GetCursorWorldPosition());
            ResetTimer();
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

    private void InitializeWeapons()
    {
        freezeAttack = true;

        for (int i = 0; i < weaponPrefabs.Count; i++)
        {
            Weapon weaponScript = weaponPrefabs[i].GetComponentInChildren<Weapon>();
            GameObject weapon = Instantiate(weaponPrefabs[i], playerMovement.transform.position
                + playerMovement.transform.right * weaponScript.GetOffsetSide(), playerMovement.transform.rotation) as GameObject;
            weapon.transform.parent = playerMovement.transform;
            weapon.transform.Translate(0.0f, weaponScript.GetOffsetHeight(), 0.0f);
            weapon.SetActive(false);
            weapons.Add(weapon);
        }

        freezeAttack = false;
    }

    private void ActivateWeapon(int weaponIndex)
    {
        freezeAttack = true;
        if (activeWeapon != null)
        {
            activeWeapon.SetActive(false);
            activeWeaponScript = null;
        }

        activeWeapon = weapons[activeWeaponIndex];
        activeWeapon.SetActive(true);
        activeWeaponScript = activeWeapon.GetComponentInChildren<Weapon>();
        freezeAttack = false;
    }

    private void SwapWeapon()
    {
        activeWeaponIndex = (activeWeaponIndex + 1) % 2;
        ActivateWeapon(activeWeaponIndex);
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
        return timer >= timeBetweenAttacks && Time.timeScale != 0 && !freezeAttack;
    }

    private void ResetTimer()
    {
        timer = 0.0f;
    }
}
