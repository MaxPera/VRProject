using System;
using UnityEngine;

public class EventDisable : MonoBehaviour
{
	private void OnEnable()
	{
		EventBus.Instance.Subscribe<ValveTurnedEvent>(DisableObject);
	}

	private void DisableObject(object sender, EventArgs eventArgs)
	{
		gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		EventBus.Instance.UnSubscribe<ValveTurnedEvent>(DisableObject);
	}
}
