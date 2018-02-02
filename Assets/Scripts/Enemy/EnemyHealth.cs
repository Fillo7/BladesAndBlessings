using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float deathDestroyDelay = 3.0f;
    [SerializeField] private int health = 50;
    private int currentHealth;

    private Animator animator;
    private Collider enemyCollider;

    private bool dead = false;
    private LinkedList<DotEffect> dotEffects = new LinkedList<DotEffect>();

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyCollider = GetComponent<Collider>();

        currentHealth = health;
    }

    void Update()
    {
        ProcessDotEffects();
    }

    public int GetCurrentHealth()
    {
        return Math.Max(0, currentHealth);
    }

    public int GetHealth()
    {
        return health;
    }

    public bool IsDead()
    {
        return dead;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= (int)amount;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (dead)
        {
            return;
        }

        int healAmount = Math.Min((int)amount, health - currentHealth);
        currentHealth += healAmount;
    }

    public void ApplyDotEffect(float duration, float tickInterval, float tickDamage)
    {
        dotEffects.AddLast(new DotEffect(duration, tickInterval, tickDamage));
    }

    private void Die()
    {
        dead = true;

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        Destroy(gameObject, deathDestroyDelay);
    }

    private void ProcessDotEffects()
    {
        LinkedList<DotEffect> toRemove = new LinkedList<DotEffect>();

        foreach (DotEffect effect in dotEffects)
        {
            effect.UpdateTimer(Time.deltaTime);

            if (effect.NextTickReady())
            {
                TakeDamage(effect.GetTickDamage());
            }

            if (effect.IsExpired())
            {
                toRemove.AddLast(effect);
            }
        }

        foreach (DotEffect effect in toRemove)
        {
            dotEffects.Remove(effect);
        }
    }
}
