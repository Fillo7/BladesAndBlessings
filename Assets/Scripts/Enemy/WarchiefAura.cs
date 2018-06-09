using UnityEngine;

public class WarchiefAura : MonoBehaviour
{
    [SerializeField] private float auraRange = 12.0f;
    [SerializeField] private float movementEffectDuration = 2.0f;
    [SerializeField] private float movemenetEffectMultiplier = 1.4f;

    private float pulseTimer = 2.0f;

    void Update()
    {
        pulseTimer += Time.deltaTime;

        if (pulseTimer > movementEffectDuration)
        {
            pulseTimer = 0.0f;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++)
            {
                if (Vector3.Distance(transform.position, enemies[i].transform.position) < auraRange)
                {
                    EnemyAI enemy = enemies[i].GetComponent<EnemyAI>();
                    if (enemy != null)
                    {
                        enemy.ApplyMovementEffect(new MovementEffect(movementEffectDuration, movemenetEffectMultiplier));
                    }
                }
            }
        }
    }
}
