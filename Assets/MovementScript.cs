using UnityEngine;

public class MovementScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public float speed = 3.0f; // Adjust this value to change the movement speed

    private void Update()
    {
        // Get the input from the left thumbstick
        Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Calculate the direction based on camera orientation
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0; // Keep movement horizontal
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Move the player
        Vector3 moveDirection = forward * primaryAxis.y + right * primaryAxis.x;
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
