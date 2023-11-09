using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollisionEmitter : SoundEmitter
{
    [SerializeField]
    private int cooldownTimer;
    private bool hasStarted = false;

    private void OnEnable()
    {
        StartCoroutine(SetBool());
    }

    private IEnumerator SetBool()
    {
        yield return new WaitForSeconds(cooldownTimer);
        hasStarted = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Collider>() && hasStarted)
        {
            AudioManager.instance.PlayEmitter(gameObject);
        }
    }
}