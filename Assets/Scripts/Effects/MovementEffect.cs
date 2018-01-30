public class MovementEffect
{
    private float duration;
    private float speedMultiplier;

    private float timer;

    public MovementEffect(float duration, float speedMultiplier)
    {
        this.duration = duration;
        this.speedMultiplier = speedMultiplier;
        timer = 0.0f;
    }

    public void UpdateTimer(float deltaTime)
    {
        timer += deltaTime;
    }

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    public bool IsExpired()
    {
        return timer >= duration;
    }
}
