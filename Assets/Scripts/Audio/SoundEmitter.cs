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

    private void Start()
    {
        AudioManager.instance.InitializeEventEmitter(eventReference, steamAudioPreset, gameObject);
    }


}