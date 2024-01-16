using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ValveInteractable : XRBaseInteractable
{
	[Range(0.0f, 1.5f)] public float TurnSpeedModifier = 1.0f;
	public int SpeedFailSafe = 300;
	[Range(0, 5)] public int RotationsForAction = 2;
	public Transform RotatableObjectTransform;
	public Axis RotationAxis = Axis.Y;
	public enum Axis
	{
		X,
		Y,
		Z
	}

	private float _currentAngle = 0.0f;
	private float _totalRotation = 0.0f;
	private bool _triggerOnce = false;

	protected override void OnSelectEntered(SelectEnterEventArgs args)
	{
		base.OnSelectEntered(args);
		_currentAngle = GetValveAngle();
	}

	protected override void OnSelectExited(SelectExitEventArgs args)
	{
		base.OnSelectExited(args);
		_currentAngle = GetValveAngle();
	}

	public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
	{
		base.ProcessInteractable(updatePhase);

		if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic) {
			if(isSelected) {
				float totalAngle = GetValveAngle();
				float angleDifference = _currentAngle - totalAngle;

				Vector3 rotationAxis = RotationAxis switch {
					Axis.X => Vector3.right,
					Axis.Y => Vector3.up,
					Axis.Z => Vector3.forward,
					_ => Vector3.up
				};

				RotatableObjectTransform.Rotate(rotationAxis, -angleDifference * TurnSpeedModifier);

				if(angleDifference < -SpeedFailSafe) {
					angleDifference = 360 + angleDifference;
				} else if(angleDifference > SpeedFailSafe) {
					angleDifference = 360 - angleDifference;
				}

				_currentAngle = totalAngle;
				_totalRotation += angleDifference;
			}
		}
	}

	private void Update()
	{
		if(!_triggerOnce && Mathf.Abs(_totalRotation) / 360 >= RotationsForAction) {
			_triggerOnce = true;
			EventBus.Instance.SendEvent(this, new ValveTurnedEvent());
		}
	}

	private float GetValveAngle()
	{
		float totalAngle = 0;

		foreach(IXRSelectInteractor interactor in interactorsSelecting) {
			Vector2 direction = transform.InverseTransformPoint(interactor.transform.position).normalized;
			totalAngle += Vector2.SignedAngle(Vector2.up, direction) * (1.0f / interactorsSelecting.Count);
		}

		return totalAngle;
	}
}
