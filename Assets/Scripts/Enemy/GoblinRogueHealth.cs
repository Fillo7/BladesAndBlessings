public class GoblinRogueHealth : EnemyHealth
{
    GoblinRogue rogueAI;

    protected override void Awake()
    {
        base.Awake();
        rogueAI = GetComponent<GoblinRogue>();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        rogueAI.TurnVisible();
    }
}
