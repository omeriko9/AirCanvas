using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log($"Still colliding with: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Stop the player movement if colliding with an obstacle
            var rigidbody = GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.linearVelocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
