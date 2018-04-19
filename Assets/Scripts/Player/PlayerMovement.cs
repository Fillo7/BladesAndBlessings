using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.5f;
    private float speedSnapshot;
    private float currentSpeed;

    private Vector3 direction;
    private Vector3 oldPosition;
    private bool movementEnabled = true;
    private bool turningLeft = false;
    private bool mouseTurning = false;

    private CustomInputManager inputManager;
    private PlayerHealth health;
    private Rigidbody playerRigidbody;
    private Animator animator;
    private LinkedList<MovementEffect> movementEffects = new LinkedList<MovementEffect>();

    private int floorMask;
    private float cameraRayLength = 100.0f;

    void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponentInChildren<CustomInputManager>();
        health = GetComponent<PlayerHealth>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        currentSpeed = speed;
        speedSnapshot = speed;
        floorMask = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        ProcessMovementEffects();
    }

    void FixedUpdate()
    {
        if (health.IsDead())
        {
            return;
        }

        float horizontal = inputManager.GetAxisRaw("Horizontal");
        float vertical = inputManager.GetAxisRaw("Vertical");

        if (!mouseTurning)
        {
            Move(horizontal, vertical);
        }
        else
        {
            MoveWithMouseTurning(horizontal, vertical);
        }

        oldPosition = transform.position;
    }

    public void LimitSpeed(float speed)
    {
        if (speed < this.speed)
        {
            this.speed = speed;
        }
    }

    public void ResetSpeed(float previousSpeedLimit)
    {
        if (Mathf.Approximately(speed, previousSpeedLimit))
        {
            speed = speedSnapshot;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetMouseTurning(bool flag)
    {
        if (!flag)
        {
            animator.SetBool("RunningBackwards", false);
        }
        mouseTurning = flag;
    }

    public void EnableMovement(bool flag)
    {
        movementEnabled = flag;

        if (!flag)
        {
            animator.SetBool("Running", false);
            animator.SetBool("TurningLeft", false);
            animator.SetBool("TurningRight", false);
        }
    }

    public void ApplyMovementEffect(MovementEffect effect)
    {
        movementEffects.AddLast(effect);
    }

    private void Move(float horizontal, float vertical)
    {
        if (TurnTowardsDirection(horizontal, vertical))
        {
            if (!movementEnabled)
            {
                return;
            }

            if (turningLeft)
            {
                animator.SetBool("TurningLeft", true);
            }
            else
            {
                animator.SetBool("TurningRight", true);
            }

            return;
        }
        else
        {
            animator.SetBool("TurningLeft", false);
            animator.SetBool("TurningRight", false);
        }

        if (!movementEnabled)
        {
            return;
        }

        if (Math.Abs(horizontal) > 0.01f || Math.Abs(vertical) > 0.01f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        direction.Set(horizontal, 0.0f, vertical);
        direction = direction.normalized * currentSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + direction);
    }

    private void MoveWithMouseTurning(float horizontal, float vertical)
    {
        if (TurnTowardsMouseCursor())
        {
            if (!movementEnabled)
            {
                return;
            }

            if (turningLeft)
            {
                animator.SetBool("TurningLeft", true);
            }
            else
            {
                animator.SetBool("TurningRight", true);
            }
        }
        else
        {
            animator.SetBool("TurningLeft", false);
            animator.SetBool("TurningRight", false);
        }

        if (!movementEnabled)
        {
            return;
        }

        if (Math.Abs(horizontal) > 0.01f || Math.Abs(vertical) > 0.01f)
        {
            Vector3 direction = transform.InverseTransformDirection(transform.position - oldPosition);
            float forwardTest = Vector3.Dot(-direction.normalized, transform.position.normalized);

            if (transform.position.z >= 0.0f && forwardTest <= 0.0f || transform.position.z < 0.0f && forwardTest > 0.0f)
            {
                animator.SetBool("Running", true);
                animator.SetBool("RunningBackwards", false);
            }
            else
            {
                animator.SetBool("RunningBackwards", true);
                animator.SetBool("Running", false);
            }
        }
        else
        {
            animator.SetBool("Running", false);
            animator.SetBool("RunningBackwards", false);
        }

        direction.Set(horizontal, 0.0f, vertical);
        direction = direction.normalized * currentSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + direction);
    }

    private bool TurnTowardsDirection(float horizontal, float vertical)
    {
        if (Math.Abs(horizontal) < 0.01f && Math.Abs(vertical) < 0.01f)
        {
            return false;
        }

        Vector3 direction = new Vector3(horizontal, 0.0f, vertical);
        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
        playerRigidbody.MoveRotation(Quaternion.RotateTowards(playerRigidbody.rotation, lookRotation, 350.0f * Time.deltaTime));

        turningLeft = GetRotationDirection(playerRigidbody.rotation, lookRotation);

        return Math.Abs(playerRigidbody.rotation.eulerAngles.y - lookRotation.eulerAngles.y) > 35.0f;
    }

    private bool TurnTowardsMouseCursor()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0.0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(Quaternion.RotateTowards(playerRigidbody.rotation, newRotation, 200.0f * Time.deltaTime));
            turningLeft = GetRotationDirection(playerRigidbody.rotation, newRotation);

            return Math.Abs(playerRigidbody.rotation.eulerAngles.y - newRotation.eulerAngles.y) > 20.0f;
        }

        return false;
    }

    private bool GetRotationDirection(Quaternion from, Quaternion to)
    {
        float fromY = from.eulerAngles.y;
        float toY = to.eulerAngles.y;
        float clockwise = 0.0f;
        float counterClockwise = 0.0f;

        if (fromY <= toY)
        {
            clockwise = toY - fromY;
            counterClockwise = fromY + (360 - toY);
        }
        else
        {
            clockwise = (360 - fromY) + toY;
            counterClockwise = fromY - toY;
        }

        // Returns true if rotating left
        return (clockwise <= counterClockwise);
    }

    private void ProcessMovementEffects()
    {
        float speedMultiplier = 1.0f;
        LinkedList<MovementEffect> toRemove = new LinkedList<MovementEffect>();

        foreach (MovementEffect effect in movementEffects)
        {
            effect.UpdateTimer(Time.deltaTime);

            speedMultiplier *= effect.GetSpeedMultiplier();

            if (effect.IsExpired())
            {
                toRemove.AddLast(effect);
            }
        }

        foreach (MovementEffect effect in toRemove)
        {
            movementEffects.Remove(effect);
        }

        currentSpeed = speed * speedMultiplier;
    }
}
