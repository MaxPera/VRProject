using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActivateAnimation : MonoBehaviour
{
	private Animator _animator;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		StartCoroutine(DelayStart());
	}

	//For demo purposes, should be changed to interaction with object later.
	private IEnumerator DelayStart()
	{
		yield return new WaitForSeconds(3);
		_animator.Play("Assemble");
		yield return new WaitForSeconds(5.5f);
		_animator.SetBool("IsBuild", true);
	}
}
