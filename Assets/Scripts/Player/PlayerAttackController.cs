using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>();
    private int activeWeaponIndex = 0;
    private float timeBetweenAttacks = 0.3f;
    private float timer = 0.0f;

    private int floorMask;
    private float cameraRayLength = 100.0f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        Sword sword = gameObject.AddComponent<Sword>();
        Bow bow = gameObject.AddComponent<Bow>();
        weapons.Add(sword);
        weapons.Add(bow);
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
            weapons[activeWeaponIndex].DoBasicAttack(target);

            // Test code start
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 location = GetCursorWorldPosition();
            cube.transform.position = new Vector3(location.x, 0.55f, location.z);
            // Test code end
        }
        else if (command == AttackCommand.Special1)
        {
            weapons[activeWeaponIndex].DoSpecialAttack1(target);
        }
        else if (command == AttackCommand.Special2)
        {
            weapons[activeWeaponIndex].DoSpecialAttack2(target);
        }
    }

    private void SwapWeapon()
    {
        activeWeaponIndex = (activeWeaponIndex + 1) % 2;
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
        return timer >= timeBetweenAttacks && Time.timeScale != 0;
    }

    private void ResetTimer()
    {
        timer = 0.0f;
    }
}
