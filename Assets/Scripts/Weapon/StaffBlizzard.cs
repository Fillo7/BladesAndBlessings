using System.Collections.Generic;
using UnityEngine;

public class StaffBlizzard : MonoBehaviour
{
    [SerializeField] private float blizzardDuration = 3.25f;
    
    private float activationDelay = 1.25f;
    private float timer = 0.0f;
    private List<EnemyAI> taggedEnemies = new List<EnemyAI>();

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blizzardDuration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy") && timer >= activationDelay)
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null && !taggedEnemies.Contains(enemy))
            {
                StaffBlizzardIce ice = other.GetComponentInChildren<StaffBlizzardIce>();

                if (ice != null)
                {
                    ice.EnableIce(8.0f);
                    enemy.ApplyMovementEffect(new MovementEffect(8.0f, 0.0f));
                }

                taggedEnemies.Add(enemy);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Enemy") && timer >= activationDelay)
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null && !taggedEnemies.Contains(enemy))
            {
                StaffBlizzardIce ice = other.GetComponentInChildren<StaffBlizzardIce>();

                if (ice != null)
                {
                    ice.EnableIce(8.0f);
                    enemy.ApplyMovementEffect(new MovementEffect(8.0f, 0.0f));
                }
                
                taggedEnemies.Add(enemy);
            }
        }
    }
}
