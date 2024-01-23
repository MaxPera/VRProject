using UnityEngine;

public class ResetRotation : MonoBehaviour
{
	[SerializeField] private GameObject _objectToRotate;

	void Start()
	{
		//Always face XR origin towards text
		_objectToRotate.transform.eulerAngles = Vector3.zero;
	}
}
