using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    private Vector3 movementInput;

    private void Start()
    {
        if (!isLocalPlayer) return;

        rb = GetComponent<Rigidbody>();
        if (rb == null) Debug.LogError("No Rigidbody found on player!");

        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smoother movement
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevents passing through objects
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        movementInput = new Vector3(moveX, 0, moveZ).normalized * speed;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer || rb == null) return;

        // Move the player smoothly with Rigidbody
        Vector3 newPosition = rb.position + movementInput * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
