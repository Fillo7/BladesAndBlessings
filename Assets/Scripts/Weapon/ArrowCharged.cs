using UnityEngine;

public class ArrowCharged : MonoBehaviour
{
    private int damage = 40;
    private float speed = 30.0f;
    private float chargeCount = 5;
    private float timeToLive = 30.0f;
    Rigidbody body;
    Vector3 currentVelocity;
    Vector3 velocitySnapshot;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }

    void Update()
    {
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        currentVelocity = body.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Projectile") || collision.gameObject.tag.Equals("Player"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            body.velocity = velocitySnapshot;
            return;
        }

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
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
            enemyHealth.TakeDamage(damage);
        }
    }

    public void SetDamage(int value)
    {
        damage = value;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    public void FollowDirection(Vector3 direction)
    {
        body.velocity = direction * speed;
        velocitySnapshot = direction * speed;
    }
}
