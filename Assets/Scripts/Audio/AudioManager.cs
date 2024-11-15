using System.Collections.Generic;
using UnityEngine;
using SteamAudio;
using FMODUnity;
using FMOD.Studio;
using System;
using System.Runtime.InteropServices;

public class AudioManager : MonoBehaviour
{
    //Creates static instance of the class to use through the project
    public static AudioManager instance { get; private set; }
    private List<StudioEventEmitter> eventEmitters;
    private List<EventInstance> eventInstances;
    [SerializeField]
    private List<string> banks = new List<string>();

    [SerializeField]
    private bool usingSteamAudio;

    [SerializeField]
    private Camera mainCamera;

    private void Awake()
    {
        //Checks if there is no instance in which case it creates a new one
        if (instance == null)
        {
            instance = this;
        }
        //If there is an instance save it
        else
            Destroy(this);

        DontDestroyOnLoad(instance);

        eventEmitters = new List<StudioEventEmitter>();
        eventInstances = new List<EventInstance>();
    }

    private void Start()
    {
        CheckSetup();
    }

    /// <summary>
    /// Checks if the Camera is setup up with the right Audio Listeners
    /// </summary>
    private void CheckSetup()
    {
        if (!TryGetComponent(out StudioBankLoader studioBankLoader))
        {
            studioBankLoader = gameObject.AddComponent<StudioBankLoader>();
            studioBankLoader.Banks = banks;
            studioBankLoader.Load();
        }

        if (!mainCamera.TryGetComponent(out StudioListener studioListener))
        {
            studioListener = mainCamera.gameObject.AddComponent<StudioListener>();
            studioListener.attenuationObject = mainCamera.gameObject;
        }
        if (!mainCamera.TryGetComponent(out SteamAudioListener steamAudioListener) && usingSteamAudio)
        {
            steamAudioListener = mainCamera.gameObject.AddComponent<SteamAudioListener>();
            steamAudioListener.applyReverb = true;
        }

    }

    /// <summary>
    /// Used to play a one shot audio event
    /// </summary>
    /// <param name="soundToPlay">What sound to play</param>
    /// <param name="worldPos">The 3D position of the sound in the Scene</param>
    public void PlayOneShot(EventReference soundToPlay, UnityEngine.Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(soundToPlay, worldPos);
    }

    /// <summary>
    /// Used to create an EventInstance used for looping
    /// </summary>
    /// <param name="eventReference">What sound to play</param>
    /// <returns></returns>
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;

    }

    /// <summary>
    /// Used to play looped audio
    /// </summary>
    /// <param name="toPlay">The event you want to loop, create beforehand using CreateInstance()</param>
    /// <param name="playCondition">The condition for the loop to activate</param>
    /// <param name="applyFadeout">Wether or not to let the loop fade out</param>
    public void PlayLoop(EventInstance toPlay,bool playCondition, bool applyFadeout)
    {
        //Checks if the audio should be played
        if (playCondition)
        {
            PLAYBACK_STATE playbackState;
            toPlay.getPlaybackState(out playbackState);
            //If there is no fade out only check for if the audio is stopped
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED) && !applyFadeout)
            {
                toPlay.start();
            }
            //If there is fade out check if the audio is stopped or stopping
            else if (playbackState.Equals(PLAYBACK_STATE.STOPPING) || playbackState.Equals(PLAYBACK_STATE.STOPPED) && applyFadeout)
            {
                toPlay.start();
            }
        }
        //Stop audio depending on fadeout mode
        else
        {
            if (applyFadeout)
            {
                toPlay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
            else
                toPlay.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
    /// <summary>
    /// Creates new sound emitter and adds it to the object the sound originates from
    /// </summary>
    /// <param name="eventReference">The sound that should be played</param>
    /// <param name="emitterGameObject">GameObject where the sound originates from, if empty initializes it on the AudioManager</param>
    /// <param name="emitterSource">If using SteamAudio add Preset</param>
    /// <returns></returns>
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, SteamAudioPreset emitterSource, GameObject emitterGameObject, bool isStatic)
    {
        StudioEventEmitter emitter = emitterGameObject.AddComponent<StudioEventEmitter>();
        SteamAudioSource source = emitterGameObject.AddComponent<SteamAudioSource>();
        emitterSource.ChangeSourceSettings(source);
        SteamAudioBakedSource bakedSource = emitterGameObject.AddComponent<SteamAudioBakedSource>();
        bakedSource.useAllProbeBatches = isStatic;
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }
    /// <summary>
    /// Creates new sound emitter and adds it to the object the sound originates from
    /// </summary>
    /// <param name="eventReference">The sound that should be played</param>
    /// <param name="emitterGameObject">GameObject where the sound originates from, if empty initializes it on the AudioManager</param>
    /// <param name="emitterSource">If using SteamAudio add Preset</param>
    /// <returns></returns>
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, SteamAudioPreset emitterSource, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.AddComponent<StudioEventEmitter>();
        if (usingSteamAudio)
        {
            SteamAudioSource source = emitterGameObject.AddComponent<SteamAudioSource>();
            if (emitterSource != null)
            {
                emitterSource.ChangeSourceSettings(source);
            }
        }
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    /// <summary>
    /// Creates new sound emitter and adds it to the object the sound originates from
    /// </summary>
    /// <param name="eventReference">The sound that should be played</param>
    /// <param name="emitterSource">If using SteamAudio add Preset</param>
    /// <returns></returns>
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, [Optional] SteamAudioPreset emitterSource)
    {
        StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
        if (usingSteamAudio)
        {
            SteamAudioSource source = gameObject.AddComponent<SteamAudioSource>();
            if (emitterSource != null)
            {
                emitterSource.ChangeSourceSettings(source);
            }
        }
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;        
    }
    /// <summary>
    /// Creates new sound emitter and adds it to the object the sound originates from
    /// </summary>
    /// <param name="eventReference">The sound that should be played</param>
    /// <param name="emitterSource">If using SteamAudio add Preset</param>
    /// <returns></returns>
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference)
    {
        StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    /// <summary>
    /// Plays emitter created by InitializeEventEmitter()
    /// </summary>
    /// <param name="thisObject">This GameObject</param>
    /// <param name="emitter">The emitter you want to play</param>
    public void PlayEmitter(GameObject thisObject)
    {
        if (!thisObject.TryGetComponent(out StudioEventEmitter emitter))
            Debug.LogError($"No StudioEventEmitter on{thisObject.name}");
        else
        {
            emitter.Play();
        }
    }
    /// <summary>
    /// Stops emitter created by InitializeEventEmitter()
    /// </summary>
    /// <param name="thisObject">This GameObject</param>
    /// <param name="emitter">The emitter you want to stop</param>
    public void StopEmitter(GameObject thisObject, StudioEventEmitter emitter)
    {
        if (!thisObject.GetComponent<StudioEventEmitter>())
            return;
        else
            emitter.Stop();
    }

    /// <summary>
    /// Let's you change the parameter of an event according to if its a float or bool
    /// </summary>
    /// <param name="eventInstance">The event you want to change the parameter for</param>
    /// <param name="parameterName">The parameter you want to change</param>
    /// <param name="parameterBoolValue">Bool to change</param>
    public void SetEventInstanceParameter(EventInstance eventInstance, string parameterName, bool parameterBoolValue)
    {
        eventInstance.setParameterByName(parameterName, Convert.ToInt32(parameterBoolValue));
    }

    /// <summary>
    /// Let's you change the parameter of an event according to if its a float or bool
    /// </summary>
    /// <param name="eventInstance">The event you want to change the parameter for</param>
    /// <param name="parameterName">The parameter you want to change</param>
    /// <param name="paramaterFloatValue">Float to change</param>
    public void SetEventInstanceParameter(EventInstance eventInstance, string parameterName, float paramaterFloatValue)
    {
        eventInstance.setParameterByName(parameterName, (float)paramaterFloatValue);
    }

    //Stops all audio on disable and clears audio cashe
    private void CleanUpAudio()
    {
        if(eventInstances.Count != 0)
            foreach (EventInstance eventInstance in eventInstances)
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        if(eventEmitters.Count != 0)
            foreach (StudioEventEmitter eventEmitter in eventEmitters)
            {
                eventEmitter.Stop();
            }
    }

    private void OnDestroy()
    {
        CleanUpAudio();
    }
}
