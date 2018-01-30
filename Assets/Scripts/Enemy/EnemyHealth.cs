﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int baseHealth = 50;
    [SerializeField] private int currentHealth;

    private Animator animator;
    private bool dead = false;
    private LinkedList<DotEffect> dotEffects = new LinkedList<DotEffect>();

    void Awake()
    {
        currentHealth = baseHealth;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        ProcessDotEffects();
    }

    public int GetCurrentHealth()
    {
        return Math.Max(0, currentHealth);
    }

    public int GetBaseHealth()
    {
        return baseHealth;
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

    public void Heal(int amount)
    {
        if (dead)
        {
            return;
        }

        int healAmount = Math.Min(amount, baseHealth - currentHealth);
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
        
        Destroy(gameObject, 2.0f);
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