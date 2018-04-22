using UnityEngine;
using UnityEngine.AI;

public class Troll : EnemyAI
{
    [SerializeField] private AnimationClip roarIntroductionClip;
    [SerializeField] private AnimationClip roarHatchlingsClip;
    [SerializeField] private AnimationClip roarBouldersClip;

    private Animator animator;
    private NavMeshObstacle obstacle;

    private bool active = false;

    private float roarHatchlingsCooldown = 45.0f;
    private float roarHatchlingsTimer = 0.0f;

    private float roarBouldersCooldown = 30.0f;
    private float roarBouldersTimer = 0.0f;

    private float globalCooldown = 5.0f;
    private float globalTimer = 5.0f;

    private bool attacking = false;

    protected override void Awake()
    {
        base.Awake();
        navigator.enabled = false;
        animator = GetComponentInChildren<Animator>();
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

        if (roarHatchlingsTimer > roarHatchlingsCooldown && globalTimer > globalCooldown)
        {
            roarHatchlingsTimer = Random.Range(0.0f, 20.0f);
            attacking = true;
            animator.SetTrigger("RoarHatchlings");
            
            Invoke("ResetAttack", roarHatchlingsClip.length);
        }
        else if (roarBouldersTimer > roarBouldersCooldown && globalTimer > globalCooldown)
        {
            roarBouldersTimer = Random.Range(0.0f, 15.0f);
            attacking = true;
            animator.SetTrigger("RoarBoulders");

            Invoke("ResetAttack", roarBouldersClip.length);
        }
        else
        {
            TurnTowardsPlayer(30.0f);
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
