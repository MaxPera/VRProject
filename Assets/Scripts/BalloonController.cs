using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public class BalloonController : MonoBehaviour
{
    public Rigidbody rb;
    private Vector3 targetAngle = new Vector3(0, 0, 90f);
    private Vector3 currentAngle;
    private bool isAttached = false;
    private float force = 15f;
    private XRGrabInteractable grabController; 

    [SerializeField]
    private float timeScale;

    private void Start()
    {
        grabController = GetComponent<XRGrabInteractable>(); 
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, 1, 0) * force);
        currentAngle = transform.eulerAngles;

        if (isAttached)
        {
            currentAngle = new Vector3(
                Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * timeScale),
                Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * timeScale),
                Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * timeScale));

            transform.eulerAngles = currentAngle;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Target")
        {
            other.gameObject.SetActive(false);
            HandleCollision();
        }
    }

    private void HandleCollision()
    {
        isAttached = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        //Disable grabbing after collision
        grabController.enabled = false; 
    }
}
