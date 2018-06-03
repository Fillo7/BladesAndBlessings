using UnityEngine;

public class RedPotion : MonoBehaviour
{
    [SerializeField] private GameObject potionFlame;

    void OnTriggerEnter(Collider other)
    {
        Instantiate(potionFlame, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
