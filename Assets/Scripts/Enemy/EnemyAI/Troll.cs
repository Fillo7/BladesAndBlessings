﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Troll : EnemyAI
{
    [SerializeField] private AudioClip breathSound;
    [SerializeField] private AudioClip roarSound;
    private AudioSource audioPlayer;

    [SerializeField] private AnimationClip roarIntroductionClip;
    [SerializeField] private AnimationClip roarHatchlingsClip;
    [SerializeField] private AnimationClip roarBouldersClip;
    [SerializeField] private AnimationClip roarBouldersPhase3Clip;
    [SerializeField] private AnimationClip breathLeftClip;
    [SerializeField] private AnimationClip breathRightClip;
    [SerializeField] private AnimationClip smashLeftClip;
    [SerializeField] private AnimationClip smashRightClip;

    [SerializeField] private List<CavePlatformController> cavePlatforms;

    private Animator animator;
    private CaveAbilities caveAbilities;
    private NavMeshObstacle obstacle;

    private bool active = false;

    private float breathSpeedMultiplier = 0.25f;
    private float smashSpeedMultiplier = 0.5f;

    private float roarHatchlingsCooldown = 35.0f;
    private float roarHatchlingsTimer = 0.0f;

    private float roarRocksCooldown = 10.0f;
    private float roarRocksTimer = 0.0f;

    private float breathCooldown = 15.0f;
    private float breathTimer = 0.0f;

    private int phase = 0;

    private bool playerHit = false;
    private bool repeatSmash = false;
    private bool lastSmashLeft = false;
    private int smashCounter = 0;
    private int maxConsecutiveSmashes = 2;
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
        animator.SetFloat("BreathSpeedMultiplier", breathSpeedMultiplier);
        animator.SetFloat("SmashSpeedMultiplier", smashSpeedMultiplier);
        caveAbilities = GetComponentInChildren<CaveAbilities>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = true;
        GetComponent<EnemyHealth>().SetImmune(true);
        audioPlayer = gameObject.AddComponent<AudioSource>();
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

        if (phase == 0)
        {
            UpdatePhase1();
        }
        else if (phase == 1)
        {
            UpdatePhase2();
        }
        else
        {
            UpdatePhase3();
        }
    }

    public void SetActive()
    {
        attacking = true;
        active = true;
        GetComponent<EnemyHealth>().SetImmune(false);

        audioPlayer.clip = roarSound;
        audioPlayer.Play();

        animator.SetTrigger("RoarIntroduction");
        Invoke("ResetAttack", roarIntroductionClip.length);
    }

    public void TriggerPhase2()
    {
        CancelInvoke();
        ResetAttack();
        repeatSmash = false;
        attacking = true;

        phase = 1;
        globalCooldown = 2.0f;
        maxConsecutiveSmashes = 3;
        breathSpeedMultiplier = 0.3f;
        animator.SetFloat("BreathSpeedMultiplier", breathSpeedMultiplier);
        smashSpeedMultiplier = 0.7f;
        animator.SetFloat("SmashSpeedMultiplier", smashSpeedMultiplier);

        audioPlayer.clip = roarSound;
        audioPlayer.Play();

        animator.SetTrigger("RoarIntroduction");
        Invoke("ResetAttack", roarIntroductionClip.length);
    }

    public void TriggerPhase3()
    {
        CancelInvoke();
        ResetAttack();
        repeatSmash = false;
        attacking = true;

        phase = 2;

        audioPlayer.clip = roarSound;
        audioPlayer.Play();

        animator.SetTrigger("RoarIntroduction");
        Invoke("ResetAttack", roarIntroductionClip.length);
    }

    private void UpdatePhase1()
    {
        if (breathTimer > breathCooldown && globalTimer > globalCooldown)
        {
            playerHit = true;
            breathTimer = Random.Range(0.0f, 5.0f);
            CavePlatformController platform = GetClosestPlatformToPlayer();

            if (platform != null)
            {
                attacking = true;

                audioPlayer.clip = breathSound;
                audioPlayer.PlayDelayed(1.0f);

                if (platform.GetCaveSide() == CaveSide.Left || (platform.GetCaveSide() == CaveSide.Middle && Random.Range(0, 2) == 0))
                {
                    animator.SetTrigger("BreathLeft");
                    Invoke("ResetAttack", breathLeftClip.length / breathSpeedMultiplier);
                }
                else
                {
                    animator.SetTrigger("BreathRight");
                    Invoke("ResetAttack", breathRightClip.length / breathSpeedMultiplier);
                }
            }
        }
        else if (roarRocksTimer > roarRocksCooldown && globalTimer > globalCooldown)
        {
            roarRocksTimer = Random.Range(0.0f, 2.5f);
            attacking = true;
            animator.SetTrigger("RoarRocks");

            audioPlayer.clip = roarSound;
            audioPlayer.Play();

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

    private void UpdatePhase2()
    {
        if (roarHatchlingsTimer > roarHatchlingsCooldown && globalTimer > globalCooldown)
        {
            roarHatchlingsTimer = Random.Range(0.0f, 10.0f);
            attacking = true;
            animator.SetTrigger("RoarHatchlings");

            audioPlayer.clip = roarSound;
            audioPlayer.Play();

            Invoke("ResetAttack", roarHatchlingsClip.length);
        }
        else if (breathTimer > breathCooldown && globalTimer > globalCooldown)
        {
            playerHit = true;
            breathTimer = Random.Range(0.0f, 5.0f);
            CavePlatformController platform = GetClosestPlatformToPlayer();

            if (platform != null)
            {
                attacking = true;

                audioPlayer.clip = breathSound;
                audioPlayer.PlayDelayed(1.0f);

                if (platform.GetCaveSide() == CaveSide.Left || (platform.GetCaveSide() == CaveSide.Middle && Random.Range(0, 2) == 0))
                {
                    animator.SetTrigger("BreathLeft");
                    Invoke("ResetAttack", breathLeftClip.length / breathSpeedMultiplier);
                }
                else
                {
                    animator.SetTrigger("BreathRight");
                    Invoke("ResetAttack", breathRightClip.length / breathSpeedMultiplier);
                }
            }
        }
        else if (roarRocksTimer > roarRocksCooldown && globalTimer > globalCooldown)
        {
            roarRocksTimer = Random.Range(0.0f, 2.5f);
            attacking = true;
            animator.SetTrigger("RoarRocks");

            audioPlayer.clip = roarSound;
            audioPlayer.Play();

            Invoke("ResetAttack", roarBouldersClip.length);
        }
        else if (smashTimer > smashCooldown && globalTimer > globalCooldown)
        {
            SmashAttack(false);
        }
        else
        {
            TurnTowardsPlayer(60.0f);
        }
    }

    private void UpdatePhase3()
    {
        if (globalTimer > globalCooldown)
        {
            attacking = true;

            audioPlayer.clip = roarSound;
            audioPlayer.Play();

            animator.SetTrigger("RoarRocksPhase3");
            Invoke("ResetAttack", roarBouldersPhase3Clip.length);
        }
        else
        {
            TurnTowardsPlayer(60.0f);
        }
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
                caveAbilities.SetTargetPlatform(targetPlatform);
                animator.SetTrigger("SmashRight");
                Invoke("ResetAttack", smashRightClip.length / smashSpeedMultiplier);
            }
            else if (chainAttack && !lastSmashLeft)
            {
                lastSmashLeft = true;
                caveAbilities.SetTargetPlatform(targetPlatform);
                animator.SetTrigger("SmashLeft");
                Invoke("ResetAttack", smashLeftClip.length / smashSpeedMultiplier);
            }
            else
            {
                if (Random.Range(0, 2) == 0)
                {
                    lastSmashLeft = true;
                    caveAbilities.SetTargetPlatform(targetPlatform);
                    animator.SetTrigger("SmashLeft");
                    Invoke("ResetAttack", smashLeftClip.length / smashSpeedMultiplier);
                }
                else
                {
                    lastSmashLeft = false;
                    caveAbilities.SetTargetPlatform(targetPlatform);
                    animator.SetTrigger("SmashRight");
                    Invoke("ResetAttack", smashRightClip.length / smashSpeedMultiplier);
                }
            }

            if (smashCounter < maxConsecutiveSmashes && Random.Range(0.0f, 1.0f) < 0.6f)
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
}
