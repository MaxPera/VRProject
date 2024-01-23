using UnityEngine;
using Unity.XR.CoreUtils;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimatorScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private Transform positionToReach;
    [SerializeField]
    private float speed;
    private void Awake()
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

    private void FixedUpdate()
    {
        if (walkingBool && positionToReach != null)
            WalkingAnimation();
    }
    /// <summary>
    /// Triggers the walking animation
    /// </summary>
    private void WalkingAnimation()
    {
        if (FindObjectOfType<XROrigin>().TryGetComponent(out XROrigin xROrigin))
            transform.LookAt(new Vector3(xROrigin.transform.position.x, transform.position.y, xROrigin.transform.position.z));

        transform.position = Vector3.MoveTowards(transform.position, positionToReach.position, speed * Time.deltaTime);
        if (transform.position == positionToReach.position)
            walkingBool = false;
    }
}
