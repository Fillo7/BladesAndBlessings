using UnityEngine;

public class CavePlatformController : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;

    private Animator animator;
    // private int health = 100;
    private bool active = true;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public bool IsActive()
    {
        return active;
    }

    public Vector3 GetCenterPoint()
    {
        return centerPoint.position;
    }

    public void Destroy()
    {
        animator.SetTrigger("Destroy");
        active = false;
    }
}
