using UnityEngine;

public class CavePlatformController : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private int health = 10;
    [SerializeField] private CaveSide caveSide = CaveSide.Left;
    [SerializeField] private bool bridge = false;
    [SerializeField] private CavePlatformController connectingPoint1;
    [SerializeField] private CavePlatformController connectingPoint2;

    [SerializeField] private Material noDamageMaterial;
    [SerializeField] private Material mediumDamageMaterial;
    [SerializeField] private Material criticalDamageMaterial;

    private Animator animator;
    private int currentHealth;
    private bool active = true;
    private bool damaged = false;
    private bool criticallyDamaged = false;

    void Awake()
    {
        ApplyMaterial(noDamageMaterial);
        animator = GetComponentInChildren<Animator>();
        currentHealth = health;

        if (bridge)
        {
            InvokeRepeating("CheckBridgeConnections", 0.25f, 0.25f);
        }
    }

    public bool IsActive()
    {
        return active;
    }

    public CaveSide GetCaveSide()
    {
        return caveSide;
    }

    public Vector3 GetCenterPoint()
    {
        return centerPoint.position;
    }

    public void TakeDamage()
    {
        currentHealth -= 1;
        float healthPercentage = currentHealth / (float)health;

        if (healthPercentage < 0.7f && !damaged && active)
        {
            damaged = true;
            ApplyMaterial(mediumDamageMaterial);
        }

        if (healthPercentage < 0.35f && !criticallyDamaged && active)
        {
            criticallyDamaged = true;
            ApplyMaterial(criticalDamageMaterial);
        }

        if (currentHealth <= 0 && active)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        animator.SetTrigger("Destroy");
        active = false;
    }

    private void ApplyMaterial(Material material)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = material;
        }
    }

    private void CheckBridgeConnections()
    {
        if ((connectingPoint1 == null || !connectingPoint1.IsActive()) && (connectingPoint2 == null || !connectingPoint2.IsActive()))
        {
            Destroy();
        }
    }
}
