using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private int baseDamage = 10;

    void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Player"))
        {
            return;
        }

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(baseDamage);
    }

    public void OnAttackBlock()
    {
        // to do
    }
}
