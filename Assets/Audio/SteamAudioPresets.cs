using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SteamAudio;
using System;
using System.Runtime.InteropServices;

public class SteamAudioPresets : MonoBehaviour
{
    public static SteamAudioPresets instance;

    private void Awake()
    {
        //Checks if there is no instance in which case it creates a new one
        if (instance == null)
            instance = this;
        //If there is an instance save it
        else
            DontDestroyOnLoad(instance);
    }

    [field: SerializeField]
    public List<SteamAudioPreset> presets { get; private set; }

    public SteamAudioPreset FindPreset(string presetName)
    {
        foreach (SteamAudioPreset aPreset in presets)
            if(presetName == aPreset.name)
                return aPreset;
        throw new Exception(presetName + "Does not excist, please put in correct name");
    }
}


[Serializable]
[CreateAssetMenu(menuName = "Steam Audio/SteamAudio Source Preset")]
public class SteamAudioPreset : ScriptableObject
{
    [field : Header("Occlusion")]
    public bool occlusion;
    public OcclusionType occlusionType;
    public bool transmission;
    [field : Header("Reflections")]
    public bool reflections;
    public ReflectionsType reflectionsType;
    public SteamAudioBakedSource bakedSource;
    [field : Header("Pathing")]
    public bool pathing;
    public SteamAudioProbeBatch probeBatch;
    public bool pathValidation;
    public bool findAlternativePaths;

    public void ChangeSourceSettings(SteamAudioSource audioSource)
    {

        audioSource.occlusion = occlusion;

        if (occlusion)
        {
            audioSource.occlusionType = occlusionType;
            audioSource.transmission = transmission;
        }

        audioSource.reflections = reflections;

        if (reflections)
        {
            audioSource.reflectionsType = reflectionsType;
            if (reflectionsType == ReflectionsType.BakedStaticSource)
                if (bakedSource != null)
                    audioSource.currentBakedSource = bakedSource;
        }

        audioSource.pathing = pathing;

        if (pathing)
        {
            if (probeBatch != null)
                audioSource.pathingProbeBatch = probeBatch;
            audioSource.pathValidation = pathValidation;
            audioSource.findAlternatePaths = findAlternativePaths;
        }
    }
}