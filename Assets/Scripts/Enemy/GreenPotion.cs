using UnityEngine;

public class GreenPotion : MonoBehaviour
{
    [SerializeField] private float healAmount = 30.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerHealth>().Heal(healAmount);
        }
        Destroy(gameObject);
    }
}
