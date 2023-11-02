using System;
using UnityEngine;

public class MazeExitScript : MonoBehaviour
{
    // Reference to the trigger object.
    public GameObject ObjectToHide;

    private void Start()
    {
        Debug.Log(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Ball"))
        {
            ObjectToHide.SetActive(false);
        }
    }
}
