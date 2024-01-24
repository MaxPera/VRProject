using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class BalloonController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 targetAngle = new Vector3(0, 0, 90f);
    private Vector3 currentAngle;
    private bool isAttached = false;

    [SerializeField]
    private float force = 15f;
    private XRGrabInteractable grabController;

    [SerializeField]
    private float lerpSpeed;

    private void Start()
    {
        grabController = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * force);
        currentAngle = transform.eulerAngles;

        if (isAttached)
        {
            currentAngle = new Vector3(
                Mathf.LerpAngle(currentAngle.x, targetAngle.x, lerpSpeed),
                Mathf.LerpAngle(currentAngle.y, targetAngle.y, lerpSpeed),
                Mathf.LerpAngle(currentAngle.z, targetAngle.z, lerpSpeed));

            transform.eulerAngles = currentAngle;
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Target")
        {
            isAttached = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            //Disable grabbing after collision
            grabController.enabled = false;
            //So this code only runs once
            other.gameObject.tag = "Untagged";
            EventBus.Instance.SendEvent(this, new BalloonEvent());
        }
    }
}
