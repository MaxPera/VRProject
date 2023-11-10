using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MazeRotationScript : MonoBehaviour
{
    [SerializeField] private int MinRotation;
    [SerializeField] private int MaxRotation;

    private float CurrentXRotation;
    private float CurrentZRotation;
    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 5f;

        CurrentXRotation = transform.eulerAngles.x >= 180 ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
        CurrentZRotation = transform.eulerAngles.z >= 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;

        if (CurrentZRotation >= MaxRotation|| CurrentZRotation <= MinRotation)
        {
            ClampZRotation();
        }

        if (CurrentXRotation >= MaxRotation || CurrentXRotation <= MinRotation)
        {
            ClampXRotation(); 
        } 
    }

    private void FixedUpdate()
    {
        transform.position = startingPosition;
    }

    private void ClampZRotation() 
    {
        CurrentZRotation = Mathf.Clamp(CurrentZRotation, MinRotation, MaxRotation);
        transform.rotation = Quaternion.Euler(CurrentXRotation, 0, CurrentZRotation);
    }

    private void ClampXRotation()
    {
        CurrentXRotation = Mathf.Clamp(CurrentXRotation, MinRotation, MaxRotation);
        transform.rotation = Quaternion.Euler(CurrentXRotation, 0, CurrentZRotation);
    }
}
