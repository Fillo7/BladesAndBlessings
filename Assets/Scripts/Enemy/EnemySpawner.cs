using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnTarget;
    [SerializeField] private bool selfDestructOnSpawn = true;
    [SerializeField] private float spawnTime = 10.0f;
    [SerializeField] private float maxTimerRandomIncrease = 0.1f;

    private float spawnTimer;
    private EnemyHealth health;

    void Awake()
    {
        health = GetComponent<EnemyHealth>();
        spawnTimer = Random.Range(0.0f, maxTimerRandomIncrease);
    }

    void Update()
    {
        if (health.IsDead())
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnTime)
        {
            Instantiate(spawnTarget, transform.position, transform.rotation);
            spawnTimer = Random.Range(0.0f, maxTimerRandomIncrease);

            if (selfDestructOnSpawn)
            {
                Destroy(gameObject);
            }
        }
    }
}
