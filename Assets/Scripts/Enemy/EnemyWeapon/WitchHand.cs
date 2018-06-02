using UnityEngine;

public class WitchHand : EnemyWeapon
{
    [SerializeField] private GameObject redPotion;

    private Transform position;
    private Transform target;

    public override void DoAttack()
    {
        Vector3 potionDirection = target.position - position.position;
        GameObject potionInstance = Instantiate(redPotion, position.position + position.forward * 1.25f + position.up * 2.0f,
            Quaternion.LookRotation(potionDirection, new Vector3(0.0f, 1.0f, 0.0f))) as GameObject;
        potionInstance.GetComponent<Rigidbody>().AddForce(2500.0f * position.up.normalized);
        potionInstance.GetComponent<Rigidbody>().AddForce(Mathf.Clamp(200.0f * potionDirection.magnitude, 1400.0f, 3600.0f) * position.forward.normalized);
    }

    public override void DoAlternateAttack()
    {}

    public override void OnAttackBlock()
    {}

    public void SetPosition(Transform position)
    {
        this.position = position;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
