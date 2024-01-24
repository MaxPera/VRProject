using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MazeExitScript : MonoBehaviour
{
    [SerializeField] private GameObject[] ObjectsToHide;
    [SerializeField] private XRGrabInteractable GrabScipt;
    [SerializeField] private Rigidbody VinylRB;
    [SerializeField] private BoxCollider VinylCollider;

    private void Start()
    {
        GrabScipt.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball")) {
            VinylCollider.enabled = false;
            GrabScipt.enabled = true;
            VinylRB.isKinematic = false;
            VinylRB.constraints = RigidbodyConstraints.None;
            foreach (GameObject obj in ObjectsToHide) {
                obj.SetActive(false);
            }
            EventBus.Instance.SendEvent(this, new MazeCompletedEvent());
        }
    }
}
