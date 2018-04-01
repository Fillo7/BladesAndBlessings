public class DoTEffect
{
    private float duration;
    private float tickInterval;
    private float tickDamage;
    private DamageType damageType;

    private float timer;
    private float tickTimer;

    public DoTEffect(float duration, float tickInterval, float tickDamage) :
        this(duration, tickInterval, tickDamage, DamageType.Bleeding)
    {}

    public DoTEffect(float duration, float tickInterval, float tickDamage, DamageType damageType)
    {
        this.duration = duration;
        this.tickInterval = tickInterval;
        this.tickDamage = tickDamage;
        this.damageType = damageType;
        timer = 0.0f;
        tickTimer = 0.0f;
    }

    public void UpdateTimer(float deltaTime)
    {
        timer += deltaTime;
        tickTimer += deltaTime;
    }

    public bool NextTickReady()
    {
        if (tickTimer < tickInterval)
        {
            return false;
        }

        tickTimer = 0.0f;
        return true;
    }

    public float GetTickDamage()
    {
        return tickDamage;
    }

    public DamageType GetDamageType()
    {
        return damageType;
    }

    public bool IsExpired()
    {
        return timer >= duration;
    }
}
