﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent navigator;
    protected EnemyHealth enemyHealth;
    protected PlayerHealth playerHealth;
    protected Transform player;

    [SerializeField] protected float movementSpeed;
    protected LinkedList<MovementEffect> movementEffects = new LinkedList<MovementEffect>();

    protected virtual void Awake()
    {
        navigator = GetComponent<NavMeshAgent>();
        navigator.speed = movementSpeed;
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void FixedUpdate()
    {
        ProcessMovementEffects();
    }

    public virtual void ApplyMovementEffect(MovementEffect effect)
    {
        movementEffects.AddLast(effect);
    }

    protected bool IsTargetInRange(Transform target, float range)
    {
        return Vector3.Distance(transform.position, target.position) < range;
    }

    protected bool IsPlayerInRange(float range)
    {
        return IsTargetInRange(player, range);
    }

    protected bool IsTargetInFront(Transform target, float halfAngle)
    {
        float angle = Vector3.Angle(transform.forward, target.position - transform.position);
        return Mathf.Abs(angle) < halfAngle;
    }

    protected bool IsPlayerInFront(float halfAngle)
    {
        return IsTargetInFront(player, halfAngle);
    }

    protected bool IsTargetInSight(Transform target)
    {
        RaycastHit hit;
        Vector3 rayDirection = target.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            return hit.transform == target;
        }
        return false;
    }

    protected bool IsPlayerInSight()
    {
        return IsTargetInSight(player);
    }

    protected float GetDistanceToTarget(Vector3 target)
    {
        return Vector3.Distance(transform.position, target);
    }

    protected float GetDistanceToTarget(Transform target)
    {
        return GetDistanceToTarget(target.position);
    }

    protected float GetDistanceToPlayer()
    {
        return GetDistanceToTarget(player);
    }

    protected void TurnTowardsTarget(Vector3 target, float turningSpeed)
    {
        foreach (MovementEffect effect in movementEffects)
        {
            if (effect.GetSpeedMultiplier() < 0.001f)
            {
                return;
            }
        }

        Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);
        lookRotation = Quaternion.Euler(0.0f, lookRotation.eulerAngles.y, 0.0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turningSpeed * Time.deltaTime);
    }

    protected void TurnTowardsTarget(Vector3 target)
    {
        TurnTowardsTarget(target, 120.0f);
    }

    protected void TurnTowardsTarget(Transform target, float turningSpeed)
    {
        TurnTowardsTarget(target.position, turningSpeed);
    }

    protected void TurnTowardsTarget(Transform target)
    {
        TurnTowardsTarget(target.position, 120.0f);
    }

    protected void TurnTowardsPlayer(float turningSpeed)
    {
        TurnTowardsTarget(player, turningSpeed);
    }

    protected void TurnTowardsPlayer()
    {
        TurnTowardsTarget(player);
    }

    protected Vector3 GetRandomLocation(float maximumDistance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * maximumDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maximumDistance, -1);

        return hit.position;
    }

    protected void ProcessMovementEffects()
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

        if (navigator.enabled)
        {
            navigator.speed = movementSpeed * speedMultiplier;
        }
    }
}
