using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private int baseHealth = 50;
    [SerializeField] private int currentHealth;

    private Animator animator;
    private bool dead;

    void Awake()
    {
        dead = false;
        currentHealth = baseHealth;
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("DummyDeath");
        Destroy(gameObject, 3.0f);
    }
}
