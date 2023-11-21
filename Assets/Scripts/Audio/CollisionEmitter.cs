using System.Collections;
using UnityEngine;

public class CollisionEmitter : VelocityEmitter
{

    private void OnEnable()
    {
        StartCoroutine(SetBool());
    }

    private IEnumerator SetBool()
    {
        yield return new WaitForSeconds(cooldownTimer);
        hasStarted = true;
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(cooldownTimer);
        canPlay = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Collider>() && hasStarted && !canPlay)
        {
            StartCoroutine(StartTimer());
            if (canPlay == true)
                StartCoroutine(PlaySound());
        }
    }
}