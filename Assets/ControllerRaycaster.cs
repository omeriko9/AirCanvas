using UnityEngine;

public class ControllerRaycaster : MonoBehaviour
{
    public Transform rayOrigin; // The starting point of the ray
    public LineRenderer lineRenderer; // Visual representation of the ray
    public float rayLength = 10f; // Maximum length of the ray
    public LayerMask canvasLayer; // Layer to detect the PaintingCanvas

    void Update()
    {
        // Set the starting point of the LineRenderer
        lineRenderer.SetPosition(0, rayOrigin.position);

        // Create the ray
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // Check if the ray hits anything on the specified layer
        if (Physics.Raycast(ray, out hit, rayLength, canvasLayer))
        {
            // Stop the ray at the point of collision
            lineRenderer.SetPosition(1, hit.point);
            OVRInput.SetControllerVibration(0.2f, 0.2f, OVRInput.Controller.RTouch); // Example for right hand

        }
        else
        {
            // Extend the ray to its maximum length if no collision occurs
            lineRenderer.SetPosition(1, rayOrigin.position + rayOrigin.forward * rayLength);
        }
    }
}
