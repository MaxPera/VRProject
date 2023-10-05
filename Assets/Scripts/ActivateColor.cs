using UnityEngine;

public class ActivateColor : MonoBehaviour
{
	private Animator _animator;
	private Renderer _renderer;
	private float _colorLevel = 0;

	private void Start()
	{
		if(!transform.parent.TryGetComponent(out _animator)) {
			Debug.LogError("No animator on parent object!");
		}

		if(gameObject.TryGetComponent(out _renderer)) {
			_renderer.material.SetFloat("_ColorLevel", _colorLevel);
		} else {
			Debug.LogError("Object has no renderer!");
		}
	}

	//For demo purposes, should be changed to interaction with object later.
	private void Update()
	{
		if(_animator.GetBool("IsBuild") && _colorLevel < 1) {
			_renderer.material.SetFloat("_ColorLevel", _colorLevel += 0.01f);
		}
	}
}
