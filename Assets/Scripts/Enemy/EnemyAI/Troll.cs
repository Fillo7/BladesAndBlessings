using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troll : EnemyAI
{
    [SerializeField] private AnimationClip roarIntroductionClip;
    [SerializeField] private AnimationClip roarHatchlingsClip;
    [SerializeField] private AnimationClip roarBouldersClip;
    [SerializeField] private AnimationClip breathLeftClip;
    [SerializeField] private AnimationClip breathRightClip;
    [SerializeField] private AnimationClip smashLeftClip;
    [SerializeField] private AnimationClip smashRightClip;

    [SerializeField] private List<CavePlatformController> cavePlatforms;

    private Animator animator;
    private NavMeshObstacle obstacle;

    private bool active = false;

    private float roarHatchlingsCooldown = 35.0f;
    private float roarHatchlingsTimer = 0.0f;

    private float roarRocksCooldown = 10.0f;
    private float roarRocksTimer = 0.0f;

    private float breathCooldown = 15.0f;
    private float breathTimer = 0.0f;

    private bool playerHit = false;
    private bool repeatSmash = false;
    private bool lastSmashLeft = false;
    private int smashCounter = 0;
    private float smashCooldown = 6.0f;
    private float smashTimer = 0.0f;
    private bool smashing = false;
    private CavePlatformController targetPlatform = null;

    private float globalCooldown = 4.0f;
    private float globalTimer = 4.0f;

    private bool attacking = false;

    protected override void Awake()
    {
        base.Awake();
        navigator.enabled = false;
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("BreathSpeedMultiplier", 0.25f);
        animator.SetFloat("SmashSpeedMultiplier", 0.5f);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !playerHit)
        {
            playerHit = true;
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(50.0f);
        }
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            obstacle.enabled = false;
            return;
        }

        if (smashing)
        {
            TurnTowardsTarget(targetPlatform.GetCenterPoint(), 60.0f);
        }

        if (attacking || !active)
        {
            return;
        }

        if (repeatSmash)
        {
            SmashAttack(true);
        }

        globalTimer += Time.deltaTime;
        roarHatchlingsTimer += Time.deltaTime;
        roarRocksTimer += Time.deltaTime;
        breathTimer += Time.deltaTime;
        smashTimer += Time.deltaTime;

        if (roarHatchlingsTimer > roarHatchlingsCooldown && globalTimer > globalCooldown)
        {
            roarHatchlingsTimer = Random.Range(0.0f, 10.0f);
            attacking = true;
            animator.SetTrigger("RoarHatchlings");
            
            Invoke("ResetAttack", roarHatchlingsClip.length);
        }
        else if (breathTimer > breathCooldown && globalTimer > globalCooldown)
        {
            breathTimer = Random.Range(0.0f, 5.0f);
            CavePlatformController platform = GetClosestPlatformToPlayer();

            if (platform != null)
            {
                attacking = true;

                if (platform.GetCaveSide() == CaveSide.Left || (platform.GetCaveSide() == CaveSide.Middle && Random.Range(0, 2) == 0))
                {
                    animator.SetTrigger("BreathLeft");
                    Invoke("ResetAttack", breathLeftClip.length / 0.25f);
                }
                else
                {
                    animator.SetTrigger("BreathRight");
                    Invoke("ResetAttack", breathRightClip.length / 0.25f);
                }
            }
        }
        else if (roarRocksTimer > roarRocksCooldown && globalTimer > globalCooldown)
        {
            roarRocksTimer = Random.Range(0.0f, 2.5f);
            attacking = true;
            animator.SetTrigger("RoarRocks");

            Invoke("ResetAttack", roarBouldersClip.length);
        }
        else if (smashTimer > smashCooldown && globalTimer > globalCooldown)
        {
            SmashAttack(false);
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

    private void SmashAttack(bool chainAttack)
    {
        smashTimer = 0.0f;
        targetPlatform = GetClosestPlatformToPlayer();

        if (targetPlatform != null)
        {
            smashCounter++;
            playerHit = false;
            attacking = true;
            smashing = true;

            if (chainAttack && lastSmashLeft)
            {
                lastSmashLeft = false;
                animator.SetTrigger("SmashRight");
                Invoke("DamagePlatform", 2.5f);
                Invoke("ResetAttack", smashRightClip.length / 0.5f);
            }
            else if (chainAttack && !lastSmashLeft)
            {
                lastSmashLeft = true;
                animator.SetTrigger("SmashLeft");
                Invoke("DamagePlatform", 2.5f);
                Invoke("ResetAttack", smashLeftClip.length / 0.5f);
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    lastSmashLeft = true;
                    animator.SetTrigger("SmashLeft");
                    Invoke("DamagePlatform", 2.5f);
                    Invoke("ResetAttack", smashLeftClip.length / 0.5f);
                }
                else
                {
                    lastSmashLeft = false;
                    animator.SetTrigger("SmashRight");
                    Invoke("DamagePlatform", 2.5f);
                    Invoke("ResetAttack", smashRightClip.length / 0.5f);
                }
            }

            if (smashCounter < 3 && Random.Range(0.0f, 1.0f) < 0.6f)
            {
                repeatSmash = true;
            }
            else
            {
                repeatSmash = false;
                smashCounter = 0;
            }
        }
    }

    private void ResetAttack()
    {
        globalTimer = 0.0f;
        smashing = false;
        attacking = false;
    }

    private CavePlatformController GetClosestPlatformToPlayer()
    {
        float shortestDistance = float.MaxValue;
        CavePlatformController result = null;

        foreach (CavePlatformController platform in cavePlatforms)
        {
            if (!platform.IsActive())
            {
                continue;
            }

            Vector3 platformCenter = platform.GetCenterPoint();
            float currentDistance = Vector3.Distance(player.position, platformCenter);

            if (currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                result = platform;
            }
        }

        return result;
    }

    private void DamagePlatform()
    {
        targetPlatform.TakeDamage();
    }
}
