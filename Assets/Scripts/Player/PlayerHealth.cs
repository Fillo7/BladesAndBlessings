using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int baseHealth = 300;
    private int currentHealth;
    [SerializeField] private Slider healthSlider;
    
    private bool dead = false;
    private LinkedList<DoTEffect> dotEffects = new LinkedList<DoTEffect>();
    bool dotClearFlag = false;
    private LinkedList<HoTEffect> hotEffects = new LinkedList<HoTEffect>();
    private Animator animator;
    private ParticleSystem bloodParticles;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        bloodParticles = GetComponentInChildren<ParticleSystem>();
        currentHealth = baseHealth;
        healthSlider.maxValue = baseHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        if (dotClearFlag)
        {
            dotEffects.Clear();
            dotClearFlag = false;
        }

        ProcessDoTEffects();
        ProcessHoTEffects();
        healthSlider.value = (float)currentHealth;
    }

    public void SetBaseHealth(int amount)
    {
        baseHealth = amount;
        currentHealth = baseHealth;
        healthSlider.maxValue = baseHealth;
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

        bloodParticles.Play();
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

        int healAmount = Math.Min((int)amount, baseHealth - currentHealth);
        currentHealth += healAmount;
    }

    public void ApplyDoTEffect(DoTEffect effect)
    {
        dotEffects.AddLast(effect);
    }

    public void ApplyHoTEffect(HoTEffect effect)
    {
        hotEffects.AddLast(effect);
    }

    public void ClearDoTEffects()
    {
        dotClearFlag = true;
    }

    private void Die()
    {
        dead = true;

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
    }

    private void ProcessDoTEffects()
    {
        LinkedList<DoTEffect> toRemove = new LinkedList<DoTEffect>();

        foreach (DoTEffect effect in dotEffects)
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

        foreach (DoTEffect effect in toRemove)
        {
            dotEffects.Remove(effect);
        }
    }

    private void ProcessHoTEffects()
    {
        LinkedList<HoTEffect> toRemove = new LinkedList<HoTEffect>();

        foreach (HoTEffect effect in hotEffects)
        {
            effect.UpdateTimer(Time.deltaTime);

            if (effect.NextTickReady())
            {
                Heal(effect.GetTickHealing());
            }

            if (effect.IsExpired())
            {
                toRemove.AddLast(effect);
            }
        }

        foreach (HoTEffect effect in toRemove)
        {
            hotEffects.Remove(effect);
        }
    }
}
