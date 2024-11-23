using UnityEngine;

public class JoystickMovement : MonoBehaviour
{
    public float baseSpeed = 5.0f; // Normal movement speed
    private float speedMultiplier = 1.5f; // Speed boost multiplier

    void Update()
    {
        // Get input for left joystick
        Vector2 leftJoystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Get input for right joystick
        Vector2 rightJoystick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Get the state of the left joystick button (click)
        bool isLeftJoystickPressed = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);

        // Calculate speed
        float currentSpeed = isLeftJoystickPressed ? baseSpeed * speedMultiplier : baseSpeed;

        // Combine movement from both joysticks
        Vector3 leftMovement = new Vector3(leftJoystick.x, 0.0f, leftJoystick.y);
        Vector3 rightMovement = new Vector3(rightJoystick.x, 0.0f, rightJoystick.y);

        // Use right joystick for rotation (optional, remove if not needed)
        float rotationSpeed = 90f; // Degrees per second
        transform.Rotate(0, rightJoystick.x * rotationSpeed * Time.deltaTime, 0);

        // Apply movement from left joystick
        transform.Translate(leftMovement * currentSpeed * Time.deltaTime, Space.Self);
    }
}
