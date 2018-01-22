using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject chargedArrow;
    private Animator animator;
    // private WeaponType weaponType = WeaponType.Ranged;
    private PlayerMovementController playerMovement;
    private int baseDamage = 15;

    private float arrowSpeed = 20.0f;

    private float arrowFanTimer = 0.0f;
    private float arrowFanCooldown = 5.0f;

    private float chargedArrowTimer = 0.0f;
    private float chargedArrowCooldown = 15.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        arrowFanTimer -= Time.deltaTime;
        chargedArrowTimer -= Time.deltaTime;
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {
        animator.SetTrigger("BasicAttack");
    }

    public override void DoSpecialAttack1(Vector3 targetPosition)
    {
        if (arrowFanTimer > 0.0f)
        {
            return;
        }

        animator.SetTrigger("SpecialAttack1");
        arrowFanTimer = arrowFanCooldown;
    }

    public override void DoSpecialAttack2(Vector3 targetPosition)
    {
        if (chargedArrowTimer > 0.0f)
        {
            return;
        }

        animator.SetTrigger("SpecialAttack2");
        chargedArrowTimer = chargedArrowCooldown;
    }

    public override float GetOffsetSide()
    {
        return 0.68f;
    }

    public override float GetOffsetHeight()
    {
        return -0.8f;
    }

    public void SpawnArrow()
    {
        GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward,
            Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        Arrow script = movingArrow.GetComponent<Arrow>();
        script.SetDamage(baseDamage);
        script.SetSpeed(arrowSpeed);
        script.FollowDirection(transform.forward);
    }

    public void SpawnArrowFan()
    {
        for (int i = -2; i < 3; i++)
        {
            GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward,
                Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f) * Quaternion.Euler(i * 25.0f, 0.0f, 0.0f)) as GameObject;
            Arrow script = movingArrow.GetComponent<Arrow>();
            script.SetDamage(baseDamage);
            script.SetSpeed(arrowSpeed);
            script.FollowDirection(movingArrow.transform.up);
        }
    }

    public void SpawnChargedArrow()
    {
        GameObject movingArrow = Instantiate(chargedArrow, transform.position + transform.forward,
            Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        ArrowCharged script = movingArrow.GetComponent<ArrowCharged>();
        script.SetDamage(baseDamage * 2);
        script.SetSpeed(arrowSpeed * 1.5f);
        script.FollowDirection(transform.forward);
    }
}
