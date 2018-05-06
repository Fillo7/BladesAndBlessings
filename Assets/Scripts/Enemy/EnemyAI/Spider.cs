using System.Collections.Generic;
using UnityEngine;

public class Spider : EnemyAI
{
    private Animator animator;

    private List<Transform> waypoints;
    private bool waypointsInitialized = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("RunningSpeedMultiplier", 3.5f);
    }

    void Update()
    {
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

        if (!waypointsInitialized)
        {
            return;
        }

        if (GetDistanceToTarget(navigator.destination) < 1.0f)
        {
            GoToNextWaypoint();
        }
    }

    public void InitializeWaypoints(List<Transform> waypoints)
    {
        this.waypoints = waypoints;
        waypointsInitialized = true;
        GoToNextWaypoint();
    }

    private void GoToNextWaypoint()
    {
        navigator.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
    }
}
