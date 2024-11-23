using UnityEngine;

public class OVRPlayerMovement : MonoBehaviour
{
    public float speed = 3.0f;           // Movement speed
    public float rotationSpeed = 50.0f; // Rotation speed

    private CharacterController characterController;
    private float pitch = 0.0f;          // Tracks up/down rotation

    private bool canMove = true;

    void Start()
    {
        // Get the CharacterController component attached to the OVRCameraRig
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            UnityEngine.Debug.LogError("CharacterController is missing! Add one to the OVRCameraRig or its parent.");
        }
    }

    void Update()
    {
        if (!canMove) return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Get input for forward/backward and strafe movement
        float moveX = Input.GetAxis("Horizontal"); // Left/Right movement (A/D or Left/Right Arrow)
        float moveZ = Input.GetAxis("Vertical");   // Forward/Backward movement (W/S or Up/Down Arrow)

        // Calculate movement direction relative to the player's orientation
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement to the CharacterController
        if (characterController != null)
        {
            characterController.Move(move * speed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        // Handle turning (yaw) left and right
        if (Input.GetKey(KeyCode.Keypad4)) // Turn left
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Keypad6)) // Turn right
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        // Handle looking up/down (pitch)
        float pitchChange = 0;
        if (Input.GetKey(KeyCode.Keypad8)) // Look up
        {
            pitchChange = -rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Keypad2)) // Look down
        {
            pitchChange = rotationSpeed * Time.deltaTime;
        }

        // Update pitch and clamp it between -45° and 45°
        pitch = Mathf.Clamp(pitch + pitchChange, -45f, 45f);

        // Apply the pitch rotation (up/down) while keeping yaw rotation
        transform.localRotation = Quaternion.Euler(pitch, transform.localEulerAngles.y, 0);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collision detected! Movement stopped.");
            canMove = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collision cleared! Movement resumed.");
            canMove = true;
        }
    }

}
