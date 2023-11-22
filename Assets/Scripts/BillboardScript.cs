using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    // Start is called before the first frame update
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
           transform.LookAt(mainCamera.transform);
            Vector3 angles = transform.eulerAngles;
           transform.eulerAngles =new Vector3(0f, angles.y, 0f);
        }
    }
}
