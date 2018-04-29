using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 20.0f;
    [SerializeField] protected float timeToLive = 15.0f;
    [SerializeField] protected ProjectileOwner owner = ProjectileOwner.Player;

    protected Rigidbody body;
    protected float timeSinceSpawn = 0.0f;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn > timeToLive)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public virtual void SetOwner(ProjectileOwner owner)
    {
        this.owner = owner;
    }

    public virtual void SetDirection(Vector3 direction)
    {
        body.velocity = direction.normalized * speed;
    }

    public virtual void ReverseDirection()
    {
        body.velocity = Quaternion.Euler(0.0f, 180.0f, 0.0f) * body.velocity;
        transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f) * transform.rotation;
    }
}
