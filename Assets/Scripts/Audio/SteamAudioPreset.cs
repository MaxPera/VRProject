using UnityEngine;
using SteamAudio;
using System;


[Serializable]
[CreateAssetMenu(menuName = "Steam Audio/SteamAudio Source Preset")]
public class SteamAudioPreset : ScriptableObject
{
    [field: Header("Occlusion")]
    public bool occlusion;
    public OcclusionType occlusionType;
    public bool transmission;
    [field: Header("Reflections")]
    public bool reflections;
    public ReflectionsType reflectionsType;
    public SteamAudioBakedSource bakedSource;
    [field: Header("Pathing")]
    public bool pathing;
    public SteamAudioProbeBatch probeBatch;
    public bool pathValidation;
    public bool findAlternativePaths;
    /// <summary>
    /// Changes the SteamAudioSource settings to the preset
    /// </summary>
    /// <param name="audioSource">What SteamAudioSource to change</param>
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
