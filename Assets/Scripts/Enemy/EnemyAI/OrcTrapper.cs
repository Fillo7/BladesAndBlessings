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

    private bool attacking = false;
    private bool turningTowardsPlayer = false;

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

        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            navigator.enabled = false;
            return;
        }

        if (movementTimer > movementCooldown && !attacking)
        {
            MoveRandomly();
        }

        if (trapTimer > trapCooldown)
        {
            SpawnTrap();
        }

        if (attackTimer > attackCooldown && IsPlayerInSight() && !attacking)
        {
            PrepareAttack();
        }

        if (turningTowardsPlayer)
        {
            TurnTowardsPlayer();
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

    private void TurnTowardsPlayer()
    {
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);
        lookRotation = Quaternion.Euler(0.0f, lookRotation.eulerAngles.y, 0.0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 150.0f * Time.deltaTime);
    }

    private void PrepareAttack()
    {
        if (navigator.enabled)
        {
            navigator.isStopped = true;
        }
        turningTowardsPlayer = true;
        attacking = true;

        Invoke("Attack", 1.5f);
    }

    private void Attack()
    {
        Vector3 arrowDirection = player.position - transform.position;
        GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward * 1.5f + transform.up,
            Quaternion.LookRotation(arrowDirection, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        Arrow script = movingArrow.GetComponent<Arrow>();
        script.SetDamage(arrowDamage);
        script.SetDirection(movingArrow.transform.up);

        attackTimer = 0.0f;
        attacking = false;
        turningTowardsPlayer = false;
        if (navigator.enabled)
        {
            navigator.isStopped = false;
        }
    }

    private void SpawnTrap()
    {
        Instantiate(trap, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
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
