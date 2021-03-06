﻿using UnityEngine;
using UnityEngine.AI;

public class Warg : EnemyAI
{
    [SerializeField] private AudioClip attackSound;
    private AudioSource audioPlayer;

    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float damage = 25.0f;
    [SerializeField] private float attackCooldown = 0.4f;

    private Animator animator;
    private NavMeshObstacle obstacle;

    private bool attacking = false;
    private bool playerHit = false;
    private float attackRange = 2.5f;
    private float attackTimer = 0.4f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("AttackSpeedMultiplier", 1.8f);
        animator.SetFloat("RunningSpeedMultiplier", 2.25f);
        animator.SetFloat("DeathSpeedMultiplier", 0.6f);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !playerHit)
        {
            playerHit = true;
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
        }
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            return;
        }

        attackTimer += Time.deltaTime;

        if (navigator.enabled && navigator.velocity.magnitude > 0.35f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (IsPlayerInRange(attackRange))
        {
            navigator.enabled = false;
            obstacle.enabled = true;

            if (IsPlayerInFront(60.0f) && attackTimer > attackCooldown && !attacking)
            {
                Attack();
            }

            TurnTowardsPlayer(180.0f);
        }
        else
        {
            if (!attacking)
            {
                obstacle.enabled = false;
                navigator.enabled = true;
            }
        }

        if (navigator.enabled)
        {
            navigator.SetDestination(player.position);
        }
    }

    public void ResetAttack()
    {
        attackTimer = 0.0f;
        obstacle.enabled = false;
        navigator.enabled = true;
        attacking = false;
    }

    private void Attack()
    {
        attacking = true;
        playerHit = false;
        navigator.enabled = false;
        obstacle.enabled = true;
        audioPlayer.clip = attackSound;
        audioPlayer.PlayDelayed(0.5f);
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length / 1.8f);
    }
}
