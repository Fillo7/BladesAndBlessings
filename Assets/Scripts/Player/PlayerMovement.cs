using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    private Vector3 direction;
    private Rigidbody playerRigidbody;

    private int floorMask;
    private float cameraRayLength = 100.0f;

	//how long is the player character snared - can't move
	private float snared = 0.0f;

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
    }

	//snares player character for the given time
	public void Snare(float lenghtOfSnare) {

		snared += lenghtOfSnare;

	}

    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

		if (snared > 0.0f) {
		
			snared -= Time.fixedDeltaTime;

		} else {

			snared = 0.0f;
			Move(horizontal, vertical);

		}
			        
        Turn();
    }

    private void Move(float horizontal, float vertical)
    {
        direction.Set(horizontal, 0.0f, vertical);
        direction = direction.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + direction);
    }

    private void Turn()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0.0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
}
