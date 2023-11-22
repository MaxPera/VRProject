using System.Collections;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class CollisionEmitter : SoundEmitter
{
    [SerializeField]
    private float cooldownTimer;
    private bool canPlay = true;
    private bool hasStarted = false;
    [SerializeField]
    private bool usesVelocity;

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
        if (usesVelocity)
        {
            if (!TryGetComponent(out Rigidbody rBody))
                Debug.LogError($"No rigidbody on{gameObject.name}");
            if (!TryGetComponent(out StudioEventEmitter eventEmitter))
                Debug.LogError($"No StudioEventEmitter on{gameObject.name}");
            EventInstance eventInstance = eventEmitter.EventInstance;
            float volumeVelocity = Mathf.Clamp01(rBody.velocity.magnitude * 50f);

            AudioManager.instance.SetEventInstanceParameter(eventInstance, "VolumeVelocity", volumeVelocity);
        }
        canPlay = false;
        yield return new WaitForEndOfFrame();
        AudioManager.instance.PlayEmitter(gameObject);
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
            if(canPlay == true)
                StartCoroutine(PlaySound());
        }
    }
}