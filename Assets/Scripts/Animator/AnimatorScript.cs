using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorScript : MonoBehaviour
{
    private Animator animator;

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
}
