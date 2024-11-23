using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public float rotationSpeed = 60.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input from the right thumbstick
        Vector2 rightAxis = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Rotate the camera rig (left/right movement only)
        if (rightAxis.x != 0)
        {
            transform.Rotate(0, rightAxis.x * rotationSpeed * Time.deltaTime, 0);
        }
    }
}
