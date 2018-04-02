using System.Collections.Generic;
using UnityEngine;

public class TorchFlames : MonoBehaviour
{
    private float flamesDamage = 4.0f;

    private float tickTime = 0.25f;
    private float tickTimer = 0.0f;

    private float flamesDuration = 2.5f;
    private float flamesTimer = 0.0f;

    private LinkedList<EnemyHealth> enemiesInFlames = new LinkedList<EnemyHealth>();

    void Update()
    {
        tickTimer += Time.deltaTime;
        flamesTimer += Time.deltaTime;

        if (tickTimer > tickTime)
        {
            ApplyDamage();
            tickTimer = 0.0f;
        }

        if (flamesTimer >= flamesDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            if (!enemiesInFlames.Contains(health))
            {
                enemiesInFlames.AddLast(health);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            if (enemiesInFlames.Contains(health))
            {
                enemiesInFlames.Remove(health);
            }
        }
    }

    public void SetDamage(float damage)
    {
        flamesDamage = damage;
    }

    public void SetDuration(float duration)
    {
        flamesDuration = duration;
    }

    private void ApplyDamage()
    {
        foreach (EnemyHealth health in enemiesInFlames)
        {
            health.TakeDamage(flamesDamage, DamageType.Fire);
        }
    }
}
