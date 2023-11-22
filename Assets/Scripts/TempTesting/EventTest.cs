using System.Collections;
using UnityEngine;

public class EventTest : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(DelayedStart());
	}

	private IEnumerator DelayedStart()
	{
		yield return new WaitForSeconds(2);
		EventBus.Instance.SendEvent(this, new ValveTurnedEvent());
	}
}
