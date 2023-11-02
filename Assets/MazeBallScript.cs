using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 lastValidPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastValidPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Check if the ball has moved through a wall in the last frame
        if (HasCollidedWithWall())
        {
            // If it has, reset the position to the last valid position
            rb.position = lastValidPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            // If not, update the last valid position
            lastValidPosition = transform.position;
        }
    }

    bool HasCollidedWithWall()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f); // Adjust the radius to match your ball size
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Wall")) // Replace "Wall" with the tag you use for your walls
            {
                return true;
            }
        }
        return false;
    }
}
