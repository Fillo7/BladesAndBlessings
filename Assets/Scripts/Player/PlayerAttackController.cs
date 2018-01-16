using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    GameObject activeWeapon = null;
    Weapon activeWeaponScript = null;
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

            // Test code start
            /*GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 location = GetCursorWorldPosition();
            cube.transform.position = new Vector3(location.x, 0.55f, location.z);*/
            // Test code end
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

    private void ActivateWeapon(int weaponIndex)
    {
        freezeAttack = true;
        if (activeWeapon != null)
        {
            Destroy(activeWeapon);
            activeWeaponScript = null;
        }

        activeWeaponScript = weapons[activeWeaponIndex].GetComponent<Weapon>();
        activeWeapon = Instantiate(weapons[weaponIndex], playerMovement.transform.position
            + playerMovement.transform.right * activeWeaponScript.GetOffsetSide(), playerMovement.transform.rotation) as GameObject;
        activeWeaponScript = activeWeapon.GetComponent<Weapon>(); // use the script which is attached to newly created weapon
        activeWeapon.transform.parent = playerMovement.transform;
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
