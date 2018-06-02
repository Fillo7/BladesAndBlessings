using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Snake : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Animator animator;
    private NavMeshObstacle obstacle;

    private List<Transform> waypoints;
    private int currentWaypointIndex = -1;
    private bool waypointsInitialized = false;

    private bool playerSighted = false;
    private float playerEngageRange = 5.0f;

    private bool attacking = false;
    private bool playerHit = false;
    private float attackRange = 2.6f;
    private float attackTimer = 1.5f;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("IdleSpeedMultiplier", 0.65f);
        animator.SetFloat("RunningSpeedMultiplier", 2.5f);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !playerHit)
        {
            playerHit = true;
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            playerHealth.ApplyDoTEffect(new DoTEffect(12.6f, 2.5f, 8.0f));
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

        if (IsPlayerInRange(playerEngageRange))
        {
            playerSighted = true;
        }

        if (!playerSighted)
        {
            if (waypointsInitialized && GetDistanceToTarget(navigator.destination) < 1.0f)
            {
                GoToNextWaypoint();
            }
        }
        else
        {
            if (IsPlayerInRange(attackRange))
            {
                navigator.enabled = false;
                obstacle.enabled = true;

                if (IsPlayerInFront(60.0f) && attackTimer > attackCooldown && !attacking)
                {
                    Attack();
                }
                else
                {
                    TurnTowardsPlayer(100.0f);
                }
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
    }

    public void InitializeWaypoints(List<Transform> waypoints)
    {
        this.waypoints = waypoints;
        waypointsInitialized = true;
        GoToNextWaypoint();
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
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", attackClip.length);
    }

    private void GoToNextWaypoint()
    {
        currentWaypointIndex++;
        currentWaypointIndex %= waypoints.Count;
        navigator.SetDestination(waypoints[currentWaypointIndex].position);
    }
}
