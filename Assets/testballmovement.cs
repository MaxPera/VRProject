using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAutoMovement : MonoBehaviour
{
    public float forwardSpeed = 2.0f;   // Forward movement speed
    public float rotationSpeed = 20.0f; // Rotation speed

    void Update()
    {
        // Move the ball forward
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Apply a slight rotation (e.g., around the Y-axis)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
