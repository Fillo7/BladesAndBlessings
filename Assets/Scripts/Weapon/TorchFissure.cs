using System.Collections.Generic;
using UnityEngine;

public class TorchFissure : MonoBehaviour
{
    [SerializeField] private float tickTime = 0.5f;
    [SerializeField] private float fissureDuration = 6.0f;

    private float fissureDamage = 7.0f;
    private float tickTimer = 0.0f;
    private float fissureTimer = 0.0f;
    private LinkedList<EnemyHealth> enemiesInFissure = new LinkedList<EnemyHealth>();

    void Update()
    {
        tickTimer += Time.deltaTime;
        fissureTimer += Time.deltaTime;

        if (tickTimer > tickTime)
        {
            ApplyDamage();
            tickTimer = 0.0f;
        }

        if (fissureTimer >= fissureDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            if (!enemiesInFissure.Contains(health))
            {
                enemiesInFissure.AddLast(health);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            if (enemiesInFissure.Contains(health))
            {
                enemiesInFissure.Remove(health);
            }
        }
    }

    public void SetDamage(float damage)
    {
        fissureDamage = damage;
    }

    private void ApplyDamage()
    {
        foreach (EnemyHealth health in enemiesInFissure)
        {
            health.TakeDamage(fissureDamage, DamageType.Fire);
        }
    }
}
