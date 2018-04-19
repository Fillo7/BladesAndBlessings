public class AbilityInfo
{
    private float cooldown;
    private float animationDuration;
    private float animationSpeedMultiplier;
    private float animationMovementMultiplier;
    private bool mouseTurningFlag;

    public AbilityInfo(float cooldown, float animationDuration) :
        this(cooldown, animationDuration, 1.0f, 1.0f, true)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier, float animationMovementMultiplier, bool mouseTurningFlag)
    {
        this.cooldown = cooldown;
        this.animationDuration = animationDuration;
        this.animationSpeedMultiplier = animationSpeedMultiplier;
        this.animationMovementMultiplier = animationMovementMultiplier;
        this.mouseTurningFlag = mouseTurningFlag;
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

    public bool GetMouseTurningFlag()
    {
        return mouseTurningFlag;
    }
}
