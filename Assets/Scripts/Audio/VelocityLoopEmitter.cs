using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityLoopEmitter : VelocityEmitter
{
    private void OnEnable()
    {
        StartCoroutine(PlayLoop());
    }

    private IEnumerator PlayLoop()
    {
        yield return new WaitUntil(() => hasStarted == true);
        StartCoroutine(PlaySound());
    }
}
