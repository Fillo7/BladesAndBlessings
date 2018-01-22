public class DotEffect
{
    private float duration;
    private float tickInterval;
    private int tickDamage;

    private float timer;
    private float tickTimer;

    public DotEffect(float duration, float tickInterval, int tickDamage)
    {
        this.duration = duration;
        this.tickInterval = tickInterval;
        this.tickDamage = tickDamage;
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

    public int GetTickDamage()
    {
        return tickDamage;
    }

    public bool IsExpired()
    {
        return timer >= duration;
    }
}
