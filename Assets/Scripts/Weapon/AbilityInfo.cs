public class AbilityInfo
{
    private float cooldown;
    private float animationDuration;

    public AbilityInfo(float cooldown, float animationDuration)
    {
        this.cooldown = cooldown;
        this.animationDuration = animationDuration;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public float GetAnimationDuration()
    {
        return animationDuration;
    }
}
