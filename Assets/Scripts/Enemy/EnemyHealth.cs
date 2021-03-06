﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private AudioClip hurtClip = null;
    [SerializeField] private AudioClip deathClip = null;
    private AudioSource audioPlayer;

    [SerializeField] private float deathDestroyDelay = 5.0f;
    [SerializeField] private bool destroyOnVictory = true;
    [SerializeField] private int health = 50;
    private int currentHealth;

    [SerializeField] private float slashingDamageMultiplier = 1.0f;
    [SerializeField] private float piercingDamageMultiplier = 1.0f;
    [SerializeField] private float fireDamageMultiplier = 1.0f;
    [SerializeField] private float magicDamageMultiplier = 1.0f;
    [SerializeField] private float bleedingDamageMultiplier = 1.0f;

    private Animator animator;
    private ParticleSystem bloodParticles;

    private bool dead = false;
    private LinkedList<DoTEffect> dotEffects = new LinkedList<DoTEffect>();

    private bool immune = false;

    virtual protected void Awake()
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

    public bool IsDestroyedOnVictory()
    {
        return destroyOnVictory;
    }

    public void SetImmune(bool flag)
    {
        immune = flag;
    }

    virtual public void TakeDamage(float amount, DamageType damageType)
    {
        float actualDamage = CalculateDamageForType(amount, damageType);

        if (actualDamage > 0.0f)
        {
            TakeDamage(actualDamage);
        }
    }

    virtual public void TakeDamage(float amount)
    {
        if (dead || immune)
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

    public void ApplyDoTEffect(DoTEffect effect)
    {
        dotEffects.AddLast(effect);
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
        LinkedList<DoTEffect> toRemove = new LinkedList<DoTEffect>();

        foreach (DoTEffect effect in dotEffects)
        {
            effect.UpdateTimer(Time.deltaTime);

            if (effect.NextTickReady())
            {
                TakeDamage(effect.GetTickDamage(), effect.GetDamageType());
            }

            if (effect.IsExpired())
            {
                toRemove.AddLast(effect);
            }
        }

        foreach (DoTEffect effect in toRemove)
        {
            dotEffects.Remove(effect);
        }
    }

    private float CalculateDamageForType(float amount, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Slashing:
                return amount * slashingDamageMultiplier;
            case DamageType.Piercing:
                return amount * piercingDamageMultiplier;
            case DamageType.Fire:
                return amount * fireDamageMultiplier;
            case DamageType.Magic:
                return amount * magicDamageMultiplier;
            case DamageType.Bleeding:
                return amount * bleedingDamageMultiplier;
            default:
                return amount;
        }
    }
}
