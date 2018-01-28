﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int baseHealth = 200;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;

    private bool dead = false;

    void Awake()
    {
        currentHealth = baseHealth;
        healthSlider.maxValue = baseHealth;
        healthSlider.value = currentHealth;
    }

    public bool IsDead()
    {
        return dead;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= (int)amount;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    private void Die()
    {
        dead = true;
    }
}
