using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.5f;
    private float speedSnapshot;
    private float currentSpeed;

    private Vector3 direction;
    private bool movementEnabled = true;
    private bool moving = false;
    private bool turningLeft = false;

    private Vector3 turningDirection;
    private bool automaticTurningEnabled = false;
    private float automaticTurningTimer = 0.0f;
    private float automaticTurningMaximum = 0.0f;

    private CustomInputManager inputManager;
    private PlayerHealth health;
    private Rigidbody playerRigidbody;
    private Animator animator;
    private LinkedList<MovementEffect> movementEffects = new LinkedList<MovementEffect>();

    void Awake()
    {
        inputManager = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponentInChildren<CustomInputManager>();
        health = GetComponent<PlayerHealth>();
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        currentSpeed = speed;
        speedSnapshot = speed;
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

        if (!automaticTurningEnabled)
        {
            Move(horizontal, vertical);
        }
        else
        {
            automaticTurningTimer += Time.deltaTime;
            TurnTowardsDirectionAutomatic();

            if (automaticTurningTimer > automaticTurningMaximum)
            {
                automaticTurningEnabled = false;
            }
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void ResetSpeed()
    {
        speed = speedSnapshot;
    }

    public bool IsMoving()
    {
        return moving;
    }

    public void EnableMovement(bool flag)
    {
        movementEnabled = flag;

        if (!flag)
        {
            moving = false;
            animator.SetBool("Running", false);
            animator.SetBool("TurningLeft", false);
            animator.SetBool("TurningRight", false);
        }
    }

    public void TurnTowardsDirection(Vector3 direction, float maximumTurningDuration)
    {
        turningDirection = direction;
        automaticTurningTimer = 0.0f;
        automaticTurningMaximum = maximumTurningDuration;
        automaticTurningEnabled = true;
    }

    public void ApplyMovementEffect(float duration, float speedMultiplier)
    {
        movementEffects.AddLast(new MovementEffect(duration, speedMultiplier));
    }

    private void Move(float horizontal, float vertical)
    {
        if (TurnTowardsDirection(horizontal, vertical))
        {
            if (!movementEnabled)
            {
                return;
            }
                
            moving = true;

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
            moving = true;
            animator.SetBool("Running", true);
        }
        else
        {
            moving = false;
            animator.SetBool("Running", false);
        }

        direction.Set(horizontal, 0.0f, vertical);
        direction = direction.normalized * currentSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + direction);
    }

    private void TurnTowardsDirectionAutomatic()
    {
        Quaternion lookRotation = Quaternion.LookRotation(turningDirection - transform.position);
        lookRotation = Quaternion.Euler(0.0f, lookRotation.eulerAngles.y, 0.0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 150.0f * Time.deltaTime);
    }

    private bool TurnTowardsDirection(float horizontal, float vertical)
    {
        if (Math.Abs(horizontal) < 0.01f && Math.Abs(vertical) < 0.01f)
        {
            return false;
        }

        Vector3 direction = new Vector3(horizontal, 0.0f, vertical);
        Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);
        playerRigidbody.MoveRotation(Quaternion.RotateTowards(playerRigidbody.rotation, lookRotation, 400.0f * Time.deltaTime));

        turningLeft = GetRotationDirection(playerRigidbody.rotation, lookRotation);

        return Math.Abs(playerRigidbody.rotation.eulerAngles.y - lookRotation.eulerAngles.y) > 40.0f;
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
