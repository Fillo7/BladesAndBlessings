﻿using UnityEngine;

public class Hatcher : EnemyAI
{
    [SerializeField] private float movementSpeed = 3.0f;

    private float maximumMovementDistance = 25.0f;

    protected override void Awake()
    {
        base.Awake();
        navigator.speed = movementSpeed;
        navigator.SetDestination(GetRandomLocation(maximumMovementDistance));
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            return;
        }

        if (navigator.enabled && GetDistanceToCurrentTarget() < 1.5f)
        {
            navigator.SetDestination(GetRandomLocation(maximumMovementDistance));
        }
    }

    private float GetDistanceToCurrentTarget()
    {
        return Vector3.Distance(transform.position, navigator.destination);
    }
}
