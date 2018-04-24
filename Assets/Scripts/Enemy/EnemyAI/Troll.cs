using UnityEngine;
using UnityEngine.AI;

public class Troll : EnemyAI
{
    [SerializeField] private AnimationClip roarIntroductionClip;
    [SerializeField] private AnimationClip roarHatchlingsClip;
    [SerializeField] private AnimationClip roarBouldersClip;
    [SerializeField] private AnimationClip breathLeftClip;

    private Animator animator;
    private NavMeshObstacle obstacle;

    private bool active = false;

    private float roarHatchlingsCooldown = 30.0f;
    private float roarHatchlingsTimer = 0.0f;

    private float roarRocksCooldown = 10.0f;
    private float roarRocksTimer = 0.0f;

    private float breathCooldown = 15.0f;
    private float breathTimer = 0.0f;

    private float globalCooldown = 4.0f;
    private float globalTimer = 4.0f;

    private bool attacking = false;

    protected override void Awake()
    {
        base.Awake();
        navigator.enabled = false;
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("BreathLeftSpeedMultiplier", 0.25f);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            obstacle.enabled = false;
            return;
        }

        if (attacking || !active)
        {
            return;
        }

        globalTimer += Time.deltaTime;
        roarHatchlingsTimer += Time.deltaTime;
        roarRocksTimer += Time.deltaTime;
        breathTimer += Time.deltaTime;

        if (roarHatchlingsTimer > roarHatchlingsCooldown && globalTimer > globalCooldown)
        {
            roarHatchlingsTimer = Random.Range(0.0f, 10.0f);
            attacking = true;
            animator.SetTrigger("RoarHatchlings");
            
            Invoke("ResetAttack", roarHatchlingsClip.length);
        }
        else if (roarRocksTimer > roarRocksCooldown && globalTimer > globalCooldown)
        {
            roarRocksTimer = Random.Range(0.0f, 2.5f);
            attacking = true;
            animator.SetTrigger("RoarRocks");

            Invoke("ResetAttack", roarBouldersClip.length);
        }
        else if (breathTimer > breathCooldown && globalTimer > globalCooldown)
        {
            breathTimer = Random.Range(0.0f, 7.5f);
            attacking = true;
            animator.SetTrigger("BreathLeft");

            Invoke("ResetAttack", breathLeftClip.length / 0.25f);
        }
        else
        {
            TurnTowardsPlayer(40.0f);
        }
    }

    public void SetActive()
    {
        attacking = true;
        active = true;
        animator.SetTrigger("RoarIntroduction");
        Invoke("ResetAttack", roarIntroductionClip.length);
    }

    private void ResetAttack()
    {
        globalTimer = 0.0f;
        attacking = false;
    }
}
