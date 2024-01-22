using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class FMODSounds : MonoBehaviour
{
    public static FMODSounds instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DontDestroyOnLoad(this);
    }

    //List of EventReferences with acces name
    [field : SerializeField]
    public List<AudioCombination> audioList { get; private set; }

    /// <summary>
    /// Returns an EventReference from the list using the name of it in the list
    /// </summary>
    /// <param name="audioName">Put in audio name</param>
    /// <returns></returns>
    public EventReference FindAudio(string audioName)
    {
        foreach (AudioCombination audioCombination in audioList)
            if (audioName == audioCombination.name)
                return audioCombination.eventReference;
            
        throw new Exception(audioName + "Does not excist, please put in correct name");
    }
}

//Struct to act in the list
[Serializable]
public struct AudioCombination
{
    public string name;
    public EventReference eventReference;
}
