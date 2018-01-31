using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private float currentSpeed;

    private int floorMask;
    private float cameraRayLength = 100.0f;

    private Vector3 direction;
    private Rigidbody playerRigidbody;
    private LinkedList<MovementEffect> movementEffects = new LinkedList<MovementEffect>();

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
        currentSpeed = speed;
    }

    void Update()
    {
        ProcessMovementEffects();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

		Move(horizontal, vertical);       
        Turn();
    }

    public void ApplyMovementEffect(float duration, float speedMultiplier)
    {
        movementEffects.AddLast(new MovementEffect(duration, speedMultiplier));
    }

    private void Move(float horizontal, float vertical)
    {
        direction.Set(horizontal, 0.0f, vertical);
        direction = direction.normalized * currentSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + direction);
    }

    private void Turn()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0.0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
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
