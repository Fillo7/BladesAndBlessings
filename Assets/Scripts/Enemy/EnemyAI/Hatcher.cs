using UnityEngine;

public class Hatcher : EnemyAI
{
    private float maximumMovementDistance = 35.0f;

    protected override void Awake()
    {
        base.Awake();
        GetComponentInChildren<Animator>().SetFloat("DefaultAnimationMultiplier", 2.0f);
        navigator.SetDestination(GetRandomLocation(maximumMovementDistance));
    }

    void Update()
    {
        if (enemyHealth.IsDead() || playerHealth.IsDead())
        {
            CancelInvoke();
            navigator.enabled = false;
            return;
        }

        if (navigator.enabled && GetDistanceToCurrentTarget() < 1.5f)
        {
            navigator.SetDestination(GetRandomLocation(maximumMovementDistance));
        }
    }

    public override void ApplyMovementEffect(MovementEffect effect)
    {
        if (effect.GetSpeedMultiplier() < 1.0f)
        {
            return;
        }
        movementEffects.AddLast(effect);
    }

    private float GetDistanceToCurrentTarget()
    {
        return Vector3.Distance(transform.position, navigator.destination);
    }
}
