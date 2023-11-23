using System;
using UnityEngine;

public class EnableOnValveRotation : MonoBehaviour
{
	private void Start()
	{
		EventBus.Instance.Subscribe<ValveTurnedEvent>(EnableObject);
		gameObject.SetActive(false);
	}

	private void EnableObject(object sender, EventArgs eventArgs)
	{
		gameObject.SetActive(true);
	}

	private void OnDestroy()
	{
		EventBus.Instance.UnSubscribe<ValveTurnedEvent>(EnableObject);
	}
}
