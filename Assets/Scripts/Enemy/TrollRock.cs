using UnityEngine;

public class TrollRock : MonoBehaviour
{
    [SerializeField] private float damage = 25.0f;

    void Awake()
    {
        GetComponent<Rigidbody>().velocity = -transform.up * 5.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
