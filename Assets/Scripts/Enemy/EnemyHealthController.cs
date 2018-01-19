using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int baseHealth = 50;
    [SerializeField] private int currentHealth;

    private Animator animator;
    private bool dead = false;

    void Awake()
    {
        currentHealth = baseHealth;
        animator = GetComponentInChildren<Animator>();
    }

    public bool IsDead()
    {
        return dead;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    private void Die()
    {
        dead = true;

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }
        
        Destroy(gameObject, 3.0f);
    }
}
