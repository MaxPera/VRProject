using System.Collections;
using UnityEngine;

public class CollisionEmitter : VelocityEmitter
{

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(cooldownTimer);
        canPlay = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Collider>() && hasStarted)
        {
            StartCoroutine(StartTimer());
            if (canPlay == true)
                StartCoroutine(PlaySound());
        }
    }
}