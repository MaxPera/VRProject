using UnityEngine;
using FMODUnity;

[HideInInspector]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField]
    protected EventReference eventReference;
    [SerializeField]
    public SteamAudioPreset steamAudioPreset;
    [SerializeField]
    protected bool isStatic;

    private void Start()
    {
        if (!isStatic)
            AudioManager.instance.InitializeEventEmitter(eventReference, steamAudioPreset, gameObject);
        else
        {
            AudioManager.instance.InitializeEventEmitter(eventReference, steamAudioPreset, gameObject, isStatic);
        }
    }


}