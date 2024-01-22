using System.Collections.Generic;
using UnityEngine;
using System;

public class SteamAudioPresets : MonoBehaviour
{
    public static SteamAudioPresets instance;

    [field: SerializeField]
    public List<SteamAudioPreset> presets { get; private set; }

    private void Awake()
    {
        //Checks if there is no instance in which case it creates a new one
        if (instance == null)
            instance = this;
        //If there is an instance save it
        else
            DontDestroyOnLoad(instance);
    }

    public SteamAudioPreset FindPreset(string presetName)
    {
        foreach (SteamAudioPreset aPreset in presets)
            if(presetName == aPreset.name)
                return aPreset;
        throw new Exception(presetName + "Does not excist, please put in correct name");
    }
}

