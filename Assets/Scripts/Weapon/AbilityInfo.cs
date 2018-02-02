public class AbilityInfo
{
    private float cooldown;
    private float animationDelay;
    private float animationDuration;

    public AbilityInfo(float cooldown, float animationDelay, float animationDuration)
    {
        this.cooldown = cooldown;
        this.animationDelay = animationDelay;
        this.animationDuration = animationDuration;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public float GetAnimationDelay()
    {
        return animationDelay;
    }

    public float GetAnimationDuration()
    {
        return animationDuration;
    }
}
