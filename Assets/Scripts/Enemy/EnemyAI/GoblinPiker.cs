﻿using UnityEngine;
using UnityEngine.AI;

public class GoblinPiker : MonoBehaviour
{
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private NavMeshAgent navigator;
    private Animator animator;

    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackCooldown = 1.2f;
    private float attackTimer = 1.2f;
    private bool attacking = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        navigator = GetComponent<NavMeshAgent>();
        navigator.speed = speed;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            navigator.enabled = false;
            return;
        }

        if (IsPlayerInRange(attackRange))
        {
            navigator.enabled = false;
        }
        else
        {
            navigator.enabled = true;
        }

        if (IsPlayerInRange(attackRange) && IsPlayerInFront(60.0f) && attackTimer > attackCooldown)
        {
            Attack();
        }

        if (attacking)
        {
            navigator.enabled = false;
        }
        else
        {
            navigator.enabled = true;
        }

        if (navigator.enabled)
        {
            navigator.SetDestination(player.position);
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        attackTimer = 0.0f;
        attacking = true;
        Invoke("ResetAttack", 2.0f);
    }

    private void ResetAttack()
    {
        attacking = false;
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) < range;
    }

    private bool IsPlayerInFront(float range)
    {
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        return Mathf.Abs(angle) < range;
    }
}
