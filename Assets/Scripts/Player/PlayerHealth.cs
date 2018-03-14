using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int baseHealth = 300;
    private int currentHealth;
    [SerializeField] private Slider healthSlider;
    
    private bool dead = false;
    private LinkedList<DotEffect> dotEffects = new LinkedList<DotEffect>();
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
        ProcessDotEffects();
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
