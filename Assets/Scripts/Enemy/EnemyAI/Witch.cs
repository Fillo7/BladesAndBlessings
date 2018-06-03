using UnityEngine;

public class Witch : EnemyAI
{
    [SerializeField] private AudioClip laughSound;
    private AudioSource audioPlayer;

    [SerializeField] private AnimationClip attackClip;
    private Animator animator;
    private WitchHand weapon;

    private float attackCooldown = 8.0f;
    private float attackTimer = 8.0f;
    private float attackRange = 18.0f;

    private bool attacking = false;

    protected override void Awake()
    {
        base.Awake();
        navigator.enabled = false;
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<WitchHand>();
        GetComponentInChildren<EnemyWeaponDelegate>().SetWeapon(weapon);
        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.clip = laughSound;
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            return;
        }

        attackTimer += Time.deltaTime;

        if (IsPlayerInRange(attackRange))
        {
            if (attackTimer > attackCooldown && !attacking)
            {
                Attack();
            }
            TurnTowardsPlayer();
        }
    }

    public void ResetAttack()
    {
        attackTimer = Random.Range(0.0f, 4.0f);
        attacking = false;
    }

    private void Attack()
    {
        attacking = true;
        weapon.SetPosition(transform);
        weapon.SetTarget(player);
        animator.SetTrigger("Attack");
        audioPlayer.Play();
        Invoke("ResetAttack", attackClip.length);
    }
}
