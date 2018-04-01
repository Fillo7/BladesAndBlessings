using UnityEngine;

public class ArrowCharged : Projectile
{
    [SerializeField] private float damage = 40.0f;
    [SerializeField] private int chargeCount = 15;

    private Vector3 currentVelocity;
    private Vector3 velocitySnapshot;

    protected override void Awake()
    {
        base.Awake();
        body.freezeRotation = true;
    }

    void FixedUpdate()
    {
        currentVelocity = body.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Projectile") || collision.gameObject.tag.Equals("Player"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>());
            body.velocity = velocitySnapshot;
            return;
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>());
            body.velocity = velocitySnapshot;
        }
        else
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 reflectedVelocity = Vector3.Reflect(currentVelocity, contact.normal);

            reflectedVelocity = Quaternion.Euler(0.0f, Random.Range(0.0f, 5.0f), 0.0f) * reflectedVelocity;
            body.velocity = new Vector3(reflectedVelocity.x, body.velocity.y, reflectedVelocity.z);
            body.velocity = body.velocity.normalized * speed;
            velocitySnapshot = body.velocity;

            transform.rotation = Quaternion.LookRotation(body.velocity, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }

        chargeCount--;

        if (chargeCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage, DamageType.Magic);
            chargeCount -= 2;
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public override void SetDirection(Vector3 direction)
    {
        base.SetDirection(direction);
        velocitySnapshot = direction * speed;
        transform.rotation = Quaternion.LookRotation(body.velocity, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f);
    }
}
