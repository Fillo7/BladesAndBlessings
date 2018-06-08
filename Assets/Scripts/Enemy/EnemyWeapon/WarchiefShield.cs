using UnityEngine;

public class WarchiefShield : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Weapon"))
        {
            PlayerAttack player = other.GetComponentInParent<PlayerAttack>();

            if (player != null)
            {
                Animator animator = other.GetComponentInParent<Animator>();
                animator.SetTrigger("Blocked");
                player.CancelInvoke();
                player.ResetAttack();
            }
        }
    }
}
