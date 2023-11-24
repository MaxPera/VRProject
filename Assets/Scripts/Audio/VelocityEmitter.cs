using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;

public class VelocityEmitter : SoundEmitter
{
    [SerializeField]
    protected float cooldownTimer;
    protected bool canPlay = true;
    protected bool hasStarted = false;
    [SerializeField]
    protected bool usesVelocity;

    protected void OnEnable()
    {
        StartCoroutine(SetBool());
    }

    protected IEnumerator SetBool()
    {
        yield return new WaitForSeconds(cooldownTimer);
        hasStarted = true;
    }

    protected IEnumerator PlaySound()
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
}
