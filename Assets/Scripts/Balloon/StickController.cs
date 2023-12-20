using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : MonoBehaviour
{
    private BalloonController parentController;
    private void Start()
    {
        if (transform.parent.TryGetComponent<BalloonController>(out BalloonController component))
        {
            parentController = component;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        parentController.OnCollisionEnter(other);
    }
}
