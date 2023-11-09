using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class CollisionEmitter : SoundEmitter
{
    [SerializeField]
    private float cooldownTimer;
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

    private IEnumerator PlaySound()
    {
        Rigidbody rBody = GetComponent<Rigidbody>();
        EventInstance eventInstance = GetComponent<StudioEventEmitter>().EventInstance;
        float volumeVelocit = Mathf.Clamp01(rBody.velocity.magnitude * 50f);

        AudioManager.instance.SetEventInstanceParameter(eventInstance, "VolumeVelocity", volumeVelocit);
        yield return new WaitForEndOfFrame();
        AudioManager.instance.PlayEmitter(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Collider>() && hasStarted)
        {
            StartCoroutine(PlaySound());
        }
    }
}