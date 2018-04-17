public class AbilityInfo
{
    private float cooldown;
    private float animationDuration;
    private float animationSpeedMultiplier;
    private float animationMovementMultiplier;
    private bool lockTurningFlag;

    public AbilityInfo(float cooldown, float animationDuration) :
        this(cooldown, animationDuration, 1.0f, 1.0f, true)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier, bool lockTurningFlag) :
        this(cooldown, animationDuration, animationSpeedMultiplier, 1.0f, lockTurningFlag)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier, float animationMovementMultiplier, bool lockTurningFlag)
    {
        this.cooldown = cooldown;
        this.animationDuration = animationDuration;
        this.animationSpeedMultiplier = animationSpeedMultiplier;
        this.animationMovementMultiplier = animationMovementMultiplier;
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

    public float GetAnimationMovementMultiplier()
    {
        return animationMovementMultiplier;
    }

    public bool IsTurningLocked()
    {
        return lockTurningFlag;
    }
}
