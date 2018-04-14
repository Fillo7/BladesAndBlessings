using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing = 3.0f;
    private Vector3 offset;
    private int environmentMask;

    void Start()
    {
        offset = transform.position - target.position;
        environmentMask = LayerMask.GetMask("Environment");
    }

    void FixedUpdate()
    {
        Vector3 targetCameraPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCameraPosition, smoothing * Time.deltaTime);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, offset.magnitude, environmentMask);

        foreach (RaycastHit hit in hits)
        {
            CameraCollisionHandler collisionHandler = hit.collider.gameObject.GetComponent<CameraCollisionHandler>();
            if (collisionHandler == null)
            {
                collisionHandler = hit.collider.gameObject.AddComponent<CameraCollisionHandler>();
            }
            collisionHandler.ResetTimer();
        }
    }
}
