﻿using UnityEngine;

public class Aberration : EnemyAI
{
    [SerializeField] private float auraRadius = 4.5f;
    [SerializeField] private float auraDamage = 8.0f;
    [SerializeField] private float tickTime = 0.25f;

    private Animator animator;

    private float movementSpeedSnapshot;
    private bool inPlayerRange = false;
    private float tickTimer = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        movementSpeedSnapshot = movementSpeed;
    }

    void Update()
    {
        if (inPlayerRange)
        {
            tickTimer += Time.deltaTime;

            if (tickTimer > tickTime)
            {
                playerHealth.TakeDamage(auraDamage);
                tickTimer = 0.0f;
            }
        }

        if (IsPlayerInRange(auraRadius) && !inPlayerRange)
        {
            inPlayerRange = true;
            movementSpeed = 0.5f;
        }
        else if (!IsPlayerInRange(auraRadius) && inPlayerRange)
        {
            inPlayerRange = false;
            tickTimer = 0.0f;
            movementSpeed = movementSpeedSnapshot;
        }

        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            return;
        }

        if (navigator.enabled && navigator.velocity.magnitude > 0.35f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (navigator.enabled)
        {
            navigator.SetDestination(player.position);
        }
    }
}
