﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shaman : MonoBehaviour
{

    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    [SerializeField] private GameObject fireball;
    [SerializeField] private int fireballDamage = 20;
    [SerializeField] private GameObject healingball;
    [SerializeField] private int healingballHeal = 40;

    [SerializeField] private float movementSpeed = 3.0f;

    [SerializeField] private float minimumDistance = 6.0f;

    [SerializeField] private float maximumDistance = 16.0f;

    [SerializeField] private float attackCooldown = 3.5f;
    private float attackTimer = 0.0f;

    private bool isRelocating = true;
    private bool attacking = false;
    private bool turning = false;
    private Transform turningTarget;

    private NavMeshAgent navigator;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();

        navigator = GetComponent<NavMeshAgent>();
        navigator.speed = movementSpeed;
        turningTarget = player;
    }

    void Update()
    {
        if (!isRelocating)
        {
            attackTimer += Time.deltaTime;
        }

        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            navigator.enabled = false;
            return;
        }

        if (isRelocating && DistanceToPlayer() > (minimumDistance + 1.0f) && DistanceToPlayer() < (maximumDistance - 1.0f))
        {
            isRelocating = false;
            navigator.enabled = false;
        }

        if (DistanceToPlayer () > maximumDistance && !attacking)
        {
            isRelocating = true;
            navigator.enabled = true;
            navigator.SetDestination(player.position);
        }

        if (DistanceToPlayer () < minimumDistance && !attacking)
        {
            isRelocating = true;	
            navigator.enabled = true;

            Vector3 fleeDirection = -(turningTarget.position - transform.position);
            navigator.SetDestination(fleeDirection.normalized * 15f);
        }

        if (attackTimer > attackCooldown && !isRelocating && !attacking)
        {
            PrepareAttack();
        }

        if (turning)
        {
            TurnTowardsLocation(turningTarget);
        }
    }

    private void TurnTowardsLocation(Transform location)
    {
        Quaternion lookRotation = Quaternion.LookRotation(location.position - transform.position);
        lookRotation = Quaternion.Euler(0.0f, lookRotation.eulerAngles.y, 0.0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 150.0f * Time.deltaTime);
    }

    private void PrepareAttack()
    {
        if (navigator.enabled)
        {
            navigator.isStopped = true;
        }
        turning = true;
        attacking = true;

        if (Random.Range(0, 2) == 0)
        {
            GameObject ally = FindWoundedAllyInRange(15.0f);

            if (ally != null)
            {
                turningTarget = ally.transform;
                Invoke("CastHealing", 1.5f);
            }
            else
            {
                turningTarget = player;
                Invoke("CastFireball", 1.5f);
            }
        }
        else
        {
            turningTarget = player;
            Invoke("CastFireball", 1.5f);
        }
    }

    private void CastHealing()
    {
        Vector3 spellDirection = turningTarget.position - transform.position;
        GameObject healingballInstance = Instantiate(healingball, transform.position + transform.forward * 2.2f + transform.up * 1.0f,
            Quaternion.LookRotation(spellDirection, new Vector3(0.0f, 1.0f, 0.0f))) as GameObject;
        EnemyHealProjectile script = healingballInstance.GetComponent<EnemyHealProjectile>();
        script.SetHeal(healingballHeal);
        script.SetOwner(ProjectileOwner.Enemy);
        script.SetDirection(healingballInstance.transform.forward);

        attackTimer = 0.0f;
        attacking = false;
        turning = false;
        if (navigator.enabled)
        {
            navigator.isStopped = false;
        }
    }

    private void CastFireball()
    {
        Vector3 spellDirection = turningTarget.position - transform.position;
        GameObject fireballInstance = Instantiate(fireball, transform.position + transform.forward * 2.2f + transform.up * 1.0f,
            Quaternion.LookRotation(spellDirection, new Vector3(0.0f, 1.0f, 0.0f))) as GameObject;
        Arrow script = fireballInstance.GetComponent<Arrow>();
        script.SetDamage(fireballDamage);
        script.SetOwner(ProjectileOwner.Enemy);
        script.SetDirection(fireballInstance.transform.forward);

        attackTimer = 0.0f;
        attacking = false;
        turning = false;
        if (navigator.enabled)
        {
            navigator.isStopped = false;
        }
    }

    private GameObject FindWoundedAllyInRange(float maximumRange)
    {
        GameObject[] allies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> alliesList = new List<GameObject>(allies);

        foreach (GameObject ally in alliesList)
        {
            if (ally == null || ally.Equals(gameObject))
            {
                continue;
            }

            EnemyHealth health = ally.GetComponent<EnemyHealth>();
            if (Vector3.Distance(transform.position, ally.transform.position) < maximumRange && !health.IsDead() && health.GetCurrentHealth() < health.GetHealth())
            {
                return ally;
            }
        }

        return null;
    }

    private bool IsPlayerInSight()
    {
        RaycastHit hit;
        Vector3 rayDirection = player.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            return hit.transform == player;
        }
        return false;
    }

    private float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }
}
