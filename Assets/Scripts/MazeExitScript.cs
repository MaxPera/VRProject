using UnityEngine;

public class MazeExitScript : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToHide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ObjectToHide.SetActive(false);
        }
    }
}
