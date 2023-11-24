using System;
using UnityEngine;

public class RaiseWaterOnValveRotation : MonoBehaviour
{
	[SerializeField][Range(0.1f, 1.0f)] private float _raiseSpeed = 0.5f;

	private float _waterLevel = 0f;
	private bool _waterRising;
	private Material _material;

	private void Start()
	{
		enabled = false;

		if(TryGetComponent(out Renderer renderer)) {
			_material = renderer.material;
			_material.SetFloat("_WaterLevel", 0f);

			EventBus.Instance.Subscribe<ValveTurnedEvent>(RaiseWater);
		} else {
			Debug.LogError("Object has no renderer! Ensure the script is on the correct object.");
		}
	}

	private void RaiseWater(object sender, EventArgs args)
	{
		_waterRising = true;
		enabled = true;
	}

	private void Update()
	{
		if(_waterLevel >= 1.0f) {
			_waterRising = false;
			enabled = false;
		}

		if(_waterRising) {
			_waterLevel += _raiseSpeed * Time.deltaTime;
			_material.SetFloat("_WaterLevel", _waterLevel);
		}
	}

	private void OnDisable()
	{
		EventBus.Instance.UnSubscribe<ValveTurnedEvent>(RaiseWater);
	}
}
