public class AbilityInfo
{
    private float cooldown;
    private float animationDuration;
    private float animationSpeedMultiplier;
    private bool lockTurningFlag;

    public AbilityInfo(float cooldown, float animationDuration) :
        this(cooldown, animationDuration, 1.0f, true)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier) :
        this(cooldown, animationDuration, animationSpeedMultiplier, true)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier, bool lockTurningFlag)
    {
        this.cooldown = cooldown;
        this.animationDuration = animationDuration;
        this.animationSpeedMultiplier = animationSpeedMultiplier;
        this.lockTurningFlag = lockTurningFlag;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public float GetAnimationDuration()
    {
        return animationDuration;
    }

    public float GetAnimationSpeedMultiplier()
    {
        return animationSpeedMultiplier;
    }

    public bool IsTurningLocked()
    {
        return lockTurningFlag;
    }
}
