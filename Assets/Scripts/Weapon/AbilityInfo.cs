public class AbilityInfo
{
    private float cooldown;
    private float animationDuration;
    private float animationSpeedMultiplier;

    public AbilityInfo(float cooldown, float animationDuration)
    {
        this.cooldown = cooldown;
        this.animationDuration = animationDuration;
        animationSpeedMultiplier = 1.0f;
    }

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier)
    {
        this.cooldown = cooldown;
        this.animationDuration = animationDuration;
        this.animationSpeedMultiplier = animationSpeedMultiplier;
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
}
