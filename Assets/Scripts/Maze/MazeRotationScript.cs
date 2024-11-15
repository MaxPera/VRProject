using UnityEngine;

public class MazeRotationScript : MonoBehaviour
{
    [SerializeField] private int MinRotation = -30;
    [SerializeField] private int MaxRotation = 30;

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
       

        CurrentXRotation = transform.eulerAngles.x >= 180 ? transform.eulerAngles.x - 360 : transform.eulerAngles.x;
        CurrentZRotation = transform.eulerAngles.z >= 180 ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        if (CurrentZRotation >= MaxRotation|| CurrentZRotation <= MinRotation)
        {
            CurrentZRotation = Mathf.Clamp(CurrentZRotation, MinRotation, MaxRotation);
            transform.rotation = Quaternion.Euler(CurrentXRotation, 0, CurrentZRotation);
        }

        if (CurrentXRotation >= MaxRotation || CurrentXRotation <= MinRotation)
        {
            CurrentXRotation = Mathf.Clamp(CurrentXRotation, MinRotation, MaxRotation);
            transform.rotation = Quaternion.Euler(CurrentXRotation, 0, CurrentZRotation);
        }
        Physics.SyncTransforms();
    }

    private void FixedUpdate()
    {
        transform.position = startingPosition;
    }
}
