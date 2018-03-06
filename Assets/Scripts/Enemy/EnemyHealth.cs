﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private AudioClip hurtClip = null;
    [SerializeField] private AudioClip deathClip = null;
    private AudioSource audioPlayer;

    [SerializeField] private float deathDestroyDelay = 5.0f;
    [SerializeField] private int health = 50;
    private int currentHealth;

    private Animator animator;
    private ParticleSystem bloodParticles;

    private bool dead = false;
    private LinkedList<DotEffect> dotEffects = new LinkedList<DotEffect>();

    void Awake()
    {
        audioPlayer = gameObject.AddComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        bloodParticles = GetComponentInChildren<ParticleSystem>();

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
        if (dead)
        {
            return;
        }

        if (bloodParticles != null)
        {
            bloodParticles.Play();
        }

        currentHealth -= (int)amount;

        if (deathClip != null && !dead && UnityEngine.Random.Range(0, 5) >= 3)
        {
            audioPlayer.clip = hurtClip;
            audioPlayer.Play();
        }

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

        if (deathClip != null)
        {
            audioPlayer.clip = deathClip;
            audioPlayer.Play();
        }

        Rigidbody body = GetComponent<Rigidbody>();

        if (body != null)
        {
            body.isKinematic = true;
        }

        Collider enemyCollider = GetComponent<Collider>();

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
