using System.Collections.Generic;
using UnityEngine;

public class StaffBlizzard : MonoBehaviour
{
    [SerializeField] private float blizzardDuration = 3.25f;

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
        if (other.tag.Equals("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null && !taggedEnemies.Contains(enemy))
            {
                enemy.ApplyMovementEffect(new MovementEffect(8.0f, 0.0f));
                taggedEnemies.Add(enemy);
            }
        }
    }
}
