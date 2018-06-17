using UnityEngine;

public class AbilityInfo
{
    private float cooldown;
    private float animationDuration;
    private float animationSpeedMultiplier;
    private float animationMovementMultiplier;
    private bool mouseTurningFlag;
    private int layerMask;
    private Sprite image;

    public AbilityInfo(float cooldown, float animationDuration) :
        this(cooldown, animationDuration, 1.0f, 1.0f, true, -1, null)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier, float animationMovementMultiplier, bool mouseTurningFlag) :
        this(cooldown, animationDuration, animationSpeedMultiplier, animationMovementMultiplier, mouseTurningFlag, -1, null)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier, float animationMovementMultiplier, bool mouseTurningFlag, int layerMask) :
        this(cooldown, animationDuration, animationSpeedMultiplier, animationMovementMultiplier, mouseTurningFlag, layerMask, null)
    {}

    public AbilityInfo(float cooldown, float animationDuration, float animationSpeedMultiplier, float animationMovementMultiplier, bool mouseTurningFlag, int layerMask, Sprite image)
    {
        this.cooldown = cooldown;
        this.animationDuration = animationDuration;
        this.animationSpeedMultiplier = animationSpeedMultiplier;
        this.animationMovementMultiplier = animationMovementMultiplier;
        this.mouseTurningFlag = mouseTurningFlag;
        this.layerMask = layerMask;
        this.image = image;
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

    public int GetLayerMask()
    {
        return layerMask;
    }

    public Sprite GetImage()
    {
        return image;
    }
}
