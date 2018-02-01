using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject chargedArrow;
    private PlayerMovement playerMovement;
    private int baseDamage = 15;

    private float arrowSpeed = 20.0f;

    private float arrowFanTimer = 5.0f;
    private float arrowFanCooldown = 5.0f;

    private float chargedArrowTimer = 15.0f;
    private float chargedArrowCooldown = 15.0f;

    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        arrowFanTimer += Time.deltaTime;
        chargedArrowTimer += Time.deltaTime;
    }

    public override void DoBasicAttack(Vector3 targetPosition)
    {}

    public override void DoSpecialAttack1(Vector3 targetPosition)
    {
        if (arrowFanTimer < arrowFanCooldown)
        {
            return;
        }

        arrowFanTimer = 0.0f;
    }

    public override float GetSpecialAttack1Timer()
    {
        return arrowFanTimer;
    }

    public override float GetSpecialAttack1Cooldown()
    {
        return arrowFanCooldown;
    }

    public override void DoSpecialAttack2(Vector3 targetPosition)
    {
        if (chargedArrowTimer < chargedArrowCooldown)
        {
            return;
        }

        chargedArrowTimer = 0.0f;
    }

    public override float GetSpecialAttack2Timer()
    {
        return chargedArrowTimer;
    }

    public override float GetSpecialAttack2Cooldown()
    {
        return chargedArrowCooldown;
    }

    public override void AdjustCooldowns(float passedTime)
    {
        arrowFanTimer += passedTime;
        chargedArrowTimer += passedTime;
    }

    public override void OnWeaponSwap()
    {}

    public void SpawnArrow()
    {
        GameObject movingArrow = Instantiate(arrow, transform.position + transform.forward,
            Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        Arrow script = movingArrow.GetComponent<Arrow>();
        script.SetDamage(baseDamage);
        script.SetSpeed(arrowSpeed);
        script.SetDirection(transform.forward);
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
            script.SetDirection(movingArrow.transform.up);
        }
    }

    public void SpawnChargedArrow()
    {
        GameObject movingArrow = Instantiate(chargedArrow, transform.position + transform.forward,
            Quaternion.LookRotation(playerMovement.transform.forward, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        ArrowCharged script = movingArrow.GetComponent<ArrowCharged>();
        script.SetDamage(baseDamage * 2);
        script.SetSpeed(arrowSpeed * 1.5f);
        script.SetDirection(transform.forward);
    }
}
