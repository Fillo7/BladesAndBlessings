using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private GameObject arrow;
    private Animator animator;
    // private WeaponType weaponType = WeaponType.Ranged;
    private PlayerMovementController playerMovement;
    private int baseDamage = 15;

    private float arrowSpeed = 10.0f;
    private float specialAttack1Timer = 0.0f;
    private float specialAttack1Cooldown = 5.0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        specialAttack1Timer -= Time.deltaTime;
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {
        animator.SetTrigger("BasicAttack");
    }

    public override void DoSpecialAttack1(Vector3 targetPosition)
    {
        if (specialAttack1Timer > 0.0f)
        {
            return;
        }

        animator.SetTrigger("SpecialAttack1");
        specialAttack1Timer = specialAttack1Cooldown;
    }

    public override void DoSpecialAttack2(Vector3 targetPosition)
    {
        // ...
    }

    public override float GetOffsetSide()
    {
        return 0.68f;
    }

    public void SpawnArrow()
    {
        GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward,
            Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        Arrow script = movingArrow.GetComponent<Arrow>();
        script.SetDamage(baseDamage);
        Rigidbody body = movingArrow.GetComponent<Rigidbody>();
        body.velocity = transform.forward * arrowSpeed;
    }

    public void SpawnArrowFan()
    {
        for (int i = -2; i < 3; i++)
        {
            GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward,
                Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f) * Quaternion.Euler(i * 25.0f, 0.0f, 0.0f)) as GameObject;
            Arrow script = movingArrow.GetComponent<Arrow>();
            script.SetDamage(baseDamage);
            Rigidbody body = movingArrow.GetComponent<Rigidbody>();
            body.velocity = movingArrow.transform.up * arrowSpeed;
        }
    }
}
