using UnityEngine;

public class HoTEffect : MonoBehaviour
{
    private float duration;
    private float tickInterval;
    private float tickHealing;

    private float timer;
    private float tickTimer;

    public HoTEffect(float duration, float tickInterval, float tickHealing)
    {
        this.duration = duration;
        this.tickInterval = tickInterval;
        this.tickHealing = tickHealing;
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

    public float GetTickHealing()
    {
        return tickHealing;
    }

    public bool IsExpired()
    {
        return timer >= duration;
    }
}
