using System.Collections.Generic;
using UnityEngine;

public class Shaman : EnemyAI
{
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private AnimationClip healingClip;
    [SerializeField] private float movementSpeed = 3.0f;
    [SerializeField] private float damage = 50.0f;
    [SerializeField] private float healing = 30.0f;
    [SerializeField] private float attackCooldown = 3.0f;

    private Animator animator;
    private ShamanStaff weapon;

    private float minimumPlayerDistance = 6.0f;
    private float maximumPlayerDistance = 15.0f;
    private float attackTimer = 0.0f;
    private bool isRelocating = true;
    private bool attacking = false;
    private Transform currentTarget;

    protected override void Awake()
    {
        base.Awake();
        navigator.speed = movementSpeed;
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<ShamanStaff>();
        weapon.Initialize(damage, healing);
        GetComponentInChildren<EnemyWeaponDelegate>().SetWeapon(weapon);
        currentTarget = player;
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            return;
        }

        if (!isRelocating)
        {
            attackTimer += Time.deltaTime;
            animator.SetBool("Running", false);
        }
        else
        {
            animator.SetBool("Running", true);
        }

        if (isRelocating && GetDistanceToPlayer() > (minimumPlayerDistance + 1.0f) && GetDistanceToPlayer() < (maximumPlayerDistance - 1.0f))
        {
            isRelocating = false;
            navigator.enabled = false;
        }

        if (GetDistanceToPlayer() > maximumPlayerDistance && !attacking)
        {
            isRelocating = true;
            navigator.enabled = true;
            navigator.SetDestination(player.position);
        }

        if (GetDistanceToPlayer() < minimumPlayerDistance && !attacking)
        {
            isRelocating = true;	
            navigator.enabled = true;

            Vector3 fleeDirection = -(player.position - transform.position);
            navigator.SetDestination(fleeDirection.normalized * 15f);
        }

        if (attackTimer > attackCooldown && !isRelocating)
        {
            if (!attacking)
            {
                PrepareAttack();
            }
            else
            {
                TurnTowardsTarget(currentTarget);
            }
        }
        else if(!isRelocating)
        {
            TurnTowardsPlayer();
        }
    }

    private void PrepareAttack()
    {
        if (navigator.enabled)
        {
            navigator.isStopped = true;
        }
        attacking = true;
        weapon.SetPosition(transform);

        int attackType = Random.Range(0, 2);
        bool allyFound = false;
        if (attackType == 0)
        {
            GameObject ally = FindWoundedAllyInRange(20.0f);

            if (ally != null)
            {
                allyFound = true;
                currentTarget = ally.transform;
                weapon.SetTarget(currentTarget);
                animator.SetTrigger("AttackAlternate");
                Invoke("ResetAttack", healingClip.length);
            }
        }

        if (attackType == 1 || !allyFound)
        {
            currentTarget = player;
            weapon.SetTarget(currentTarget);
            animator.SetTrigger("Attack");
            Invoke("ResetAttack", attackClip.length);
        }
    }

    private void ResetAttack()
    {
        attackTimer = 0.0f;
        attacking = false;
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
            if (IsTargetInRange(ally.transform, maximumRange) && !health.IsDead() && health.GetCurrentHealth() < health.GetHealth())
            {
                return ally;
            }
        }

        return null;
    }
}
