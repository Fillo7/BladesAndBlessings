using System.Collections.Generic;
using UnityEngine;

public class TorchFlames : MonoBehaviour
{
    private float flamesDamage = 10.0f;

    private float flamesDuration = 2.0f;
    private float flamesTimer = 0.0f;

    private int hitCount = 0;
    private List<EnemyHealth> hitEnemies = new List<EnemyHealth>();

    void Update()
    {
        flamesTimer += Time.deltaTime;

        if (flamesTimer >= flamesDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy") && hitCount < 3)
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();

            if (!hitEnemies.Contains(health))
            {
                hitCount++;
                health.TakeDamage(flamesDamage, DamageType.Fire);
                hitEnemies.Add(health);
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
}
