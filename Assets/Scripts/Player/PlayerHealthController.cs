﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private int baseHealth = 200;
    [SerializeField] private int currentHealth;
    [SerializeField] private Slider healthSlider;

    private bool dead;

    void Awake()
    {
        dead = false;
        currentHealth = baseHealth;
    }
    
    void Update()
    {
        // ...
    }

    public bool IsDead()
    {
        return dead;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    private void Die()
    {
        dead = true;
        // ...
    }
}
