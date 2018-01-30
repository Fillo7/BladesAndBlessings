using UnityEngine;
using UnityEngine.AI;

public class OrcTrapper : MonoBehaviour
{
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private NavMeshAgent navigator;
    [SerializeField] private float maximumMovementDistance = 15.0f;

    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject trap;

    [SerializeField] private int arrowDamage = 25;
    [SerializeField] private float speed = 6.0f;
    [SerializeField] private float movementCooldown = 3.0f;
    private float movementTimer = 3.0f;
    [SerializeField] private float attackCooldown = 6.0f;
    private float attackTimer = 2.0f;
    [SerializeField] private float trapCooldown = 10.0f;
    private float trapTimer = 5.0f;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();

        navigator = GetComponent<NavMeshAgent>();
        navigator.speed = speed;
    }

    void Update()
    {
        movementTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
        trapTimer += Time.deltaTime;

        if (enemyHealth.GetCurrentHealth() <= 0 || playerHealth.IsDead())
        {
            navigator.enabled = false;
            return;
        }

        if (movementTimer > movementCooldown)
        {
            MoveRandomly();
        }

        if (trapTimer > trapCooldown)
        {
            SpawnTrap();
        }

        if (attackTimer > attackCooldown && IsPlayerInSight())
        {
            Attack();
        }
    }

    private void MoveRandomly()
    {
        movementTimer = 0.0f;

        if (navigator.enabled)
        {
            navigator.SetDestination(GetRandomPosition(maximumMovementDistance));
        }
    }

    private void EnableMovement()
    {
        if (navigator.enabled)
        {
            navigator.isStopped = false;
        }
        movementTimer = 3.0f;
    }

    private void Attack()
    {
        navigator.isStopped = true;
        Vector3 arrowDirection = player.position - transform.position;
        transform.LookAt(player.transform);

        GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward * 1.5f,
            Quaternion.LookRotation(arrowDirection, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        Arrow script = movingArrow.GetComponent<Arrow>();
        script.SetDamage(arrowDamage);
        script.SetOwner(ProjectileOwner.Enemy);
        script.SetDirection(movingArrow.transform.up);
        attackTimer = 0.0f;
        Invoke("EnableMovement", 1.0f);
    }

    private void SpawnTrap()
    {
        Instantiate(trap, new Vector3(transform.position.x, 0.0f, transform.position.z), Quaternion.identity);
        trapTimer = 0.0f;
    }

    private Vector3 GetRandomPosition(float maximumDistance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * maximumDistance;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, maximumDistance, -1);

        return hit.position;
    }

    private bool IsPlayerInSight()
    {
        RaycastHit hit;
        Vector3 rayDirection = player.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            return hit.transform == player;
        }
        return false;
    }

    private bool IsPlayerInFront(float range)
    {
        float angle = Vector3.Angle(transform.forward, player.position - transform.position);
        return Mathf.Abs(angle) < range;
    }
}
