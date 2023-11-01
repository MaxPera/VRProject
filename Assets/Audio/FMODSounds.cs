using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;

public class FMODSounds : MonoBehaviour
{
    public static FMODSounds instance { get; private set; }
    [field: Header("Test SFX")]
    [field: SerializeField] public EventReference soundEffects { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DontDestroyOnLoad(this);
    }
}
