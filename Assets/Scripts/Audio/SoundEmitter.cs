using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[HideInInspector]
public class SoundEmitter : MonoBehaviour
{
    [SerializeField]
    protected EventReference eventReference;
    [SerializeField]
    protected SteamAudioPreset steamAudioPreset;
    [SerializeField]
    protected bool isStatic;

    private void Start()
    {
        if (!isStatic)
            AudioManager.instance.InitializeEventEmitter(eventReference, steamAudioPreset, gameObject);
        else
            AudioManager.instance.InitializeStaticEventEmitter(eventReference, steamAudioPreset, gameObject);
    }


}