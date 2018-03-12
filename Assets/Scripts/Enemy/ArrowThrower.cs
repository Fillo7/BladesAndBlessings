using UnityEngine;

public class ArrowThrower : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private float damage = 20.0f;

    private float arrowTimer;
    private float arrowCooldown = 5.0f;
    private EnemyHealth health;

    void Awake()
    {
        health = GetComponent<EnemyHealth>();
        arrowTimer = Random.Range(2.0f, 5.0f);
    }

    void Update()
    {
        if (health.IsDead())
        {
            return;
        }

        arrowTimer += Time.deltaTime;

        if (arrowTimer > arrowCooldown)
        {
            SpawnArrow();
            arrowTimer = 0.0f;
        }
    }

    private void SpawnArrow()
    {
        GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward + transform.up * 1.8f,
            Quaternion.LookRotation(transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        DamageProjectile script = movingArrow.GetComponent<DamageProjectile>();
        script.SetDirection(transform.forward);
        script.SetDamage(damage);
    }
}
