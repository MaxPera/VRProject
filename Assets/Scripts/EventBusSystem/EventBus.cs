using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventBus : MonoBehaviour
{
	private static EventBus _instance;
	public static EventBus Instance { get { return _instance; } }

	private Dictionary<Type, List<EventHandler>> _subscribersByType;

	private void Awake()
	{
		if(_instance != null && _instance != this) {
			Destroy(this);
		} else {
			_instance = this;
		}

		_instance._subscribersByType = new Dictionary<Type, List<EventHandler>>();
	}

	public void Subscribe<T>(EventHandler eventHandler) where T : EventArgs
	{
		var type = typeof(T);
		if(!_instance._subscribersByType.TryGetValue(type, out List<EventHandler> subscribers)) {
			_instance._subscribersByType.Add(type, new List<EventHandler>());
		}

		_instance._subscribersByType[type].Add(eventHandler);
	}

	public void UnSubscribe<T>(EventHandler eventHandler) where T : EventArgs
	{
		var type = typeof(T);
		if(_instance._subscribersByType.TryGetValue(type, out List<EventHandler> subscribers)) {
			_instance._subscribersByType[type].Remove(eventHandler);
		}
	}

	public void SendEvent(object sender, EventArgs eventArgs)
	{
		if(_instance._subscribersByType.TryGetValue(eventArgs.GetType(), out List<EventHandler> subscribers)) {
			foreach(EventHandler subscriber in subscribers.ToList()) {
				subscriber(sender, eventArgs);
			}
		}
	}
}
