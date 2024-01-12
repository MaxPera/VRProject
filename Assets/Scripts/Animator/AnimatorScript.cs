using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatorScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private Transform positionToReach;
    [SerializeField]
    private float speed;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool walkingBool
    {
        get { return animator.GetBool("Walking"); }
        set { animator.SetBool("Walking", value); }
    }
    public bool talkingBool
    {
        get { return animator.GetBool("Talking"); }
        set { animator.SetBool("Talking", value); }
    }
    public bool handUpBool
    {
        get { return animator.GetBool("HandUp"); }
        set { animator.SetBool("HandUp", value); }
    }

    public void StartWalking()
    {
        StartCoroutine(WalkingAnimation());
    }
    private IEnumerator WalkingAnimation()
    {
        walkingBool = true;
        while (transform.position != positionToReach.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToReach.position, speed * Time.deltaTime);
        }
        yield return new WaitUntil(() => transform.position == positionToReach.position);
        walkingBool = false;
    }
}
