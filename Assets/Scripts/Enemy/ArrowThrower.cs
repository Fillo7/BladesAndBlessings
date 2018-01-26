using UnityEngine;

public class ArrowThrower : MonoBehaviour
{
    [SerializeField] private GameObject arrow;

    private float nextArrowTimer;
    private float arrowCooldown = 5.0f;

    void Start()
    {
        nextArrowTimer = Random.Range(1.0f, 5.0f);
    }

	void Update()
    {
        nextArrowTimer -= Time.deltaTime;

        if (nextArrowTimer <= 0.0f)
        {
            spawnArrow();
            nextArrowTimer = arrowCooldown;
        }
	}

    private void spawnArrow()
    {
        GameObject movingArrow = Instantiate(arrow, transform.position - transform.up,
            Quaternion.LookRotation(-transform.up, new Vector3(1.0f, 0.0f, 0.0f)) * Quaternion.Euler(90.0f, 0.0f, 0.0f)) as GameObject;
        Arrow script = movingArrow.GetComponent<Arrow>();
        script.FollowDirection(-transform.up);
        script.SetOwner(ProjectileOwner.Enemy);
    }
}
