using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.IO;
using System;


public class ResearchAudioPlayer : MonoBehaviour
{
    public Transform thisTransform;
    public EventReference eventReference, pauseSound;
    public List<AudioRound<Vector3, Vector3>> audioRounds = new List<AudioRound<Vector3, Vector3>>();
    private List<AudioRound<Vector3, Vector3>> usedRound = new List<AudioRound<Vector3, Vector3>>();
    AudioRound<Vector3, Vector3> thisRound;
    //public List<AudioRound<AudioRound<Vector3, Vector3>, Vector3>> pickList = new List<AudioRound<AudioRound<Vector3, Vector3>, Vector3>>();
    public List<Vector3> pickList = new List<Vector3>();
    int counter = 0;

    public static ResearchAudioPlayer instance;
    InputActionMap leftMap, rightMap;
    public InputActionAsset input;
    public bool pickedSide;
    public bool canPick;
    public string currentName;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ConfigureHand();
        StartCoroutine(StartRound());
    }
    void ConfigureHand()
    {
            leftMap = input.FindActionMap("XRI LeftHand Interaction");

            rightMap = input.FindActionMap("XRI RightHand Interaction");
    }
    IEnumerator StartRound()
    {
        thisRound = audioRounds[UnityEngine.Random.Range(0, audioRounds.Count - 1)];
        yield return PlaySound(thisRound);
        counter++;
        canPick = true;
        yield return new WaitUntil(() => pickedSide == true);
        StartCoroutine(StartRound());
    }
    public IEnumerator PlaySound(AudioRound<Vector3,Vector3> thisRound)
    {
        pickedSide = false;
        yield return new WaitForSeconds(4);
        AudioManager.instance.PlayOneShot(eventReference, thisRound.Value);
        Debug.Log(thisRound.Value);
        yield return new WaitForSeconds(4);
        AudioManager.instance.PlayOneShot(eventReference, thisRound.Key);
        Debug.Log(thisRound.Key);
        yield return null;
    }

    private void FixedUpdate()
    {
        if (canPick)
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        if (leftMap.actions[2].WasPerformedThisFrame())
        {
            pickList.Add(thisRound.Key);
            canPick = false;
            pickedSide = true;
        }
        if (rightMap.actions[2].WasPerformedThisFrame())
        {
            pickList.Add(thisRound.Value);
            canPick = false;
            pickedSide = true;
        }
    }

    void SaveNewFile()
    {
        using (StreamWriter outputFile = File.CreateText(Application.dataPath +$"/Test{currentName}.txt"))
        {
            foreach (Vector3 line in pickList)
                outputFile.WriteLine(line.ToString());
        }

        if (File.Exists(Application.dataPath + $"Test{currentName}.txt"))
            Debug.Log("File Saved Succesfully");
    }


    private void OnDisable()
    {
        SaveNewFile();
    }
}



[System.Serializable]
public struct AudioRound<TK, TV>
{
    [SerializeField] TK key;
    [SerializeField] TV value;

    public TK Key { get => key; }
    public TV Value { get => value; }

    public AudioRound(TK key, TV value)
    {
        this.key = key;
        this.value = value;
    }
    public void SetKey(TK key)
    {
        this.key = key;
    }
    public void SetValue(TV value)
    {
        this.value = value;
    }
    public bool TryGetValue(TK key, out TV value)
    {
        value = this.value;
        if (this.key.Equals(key))
            return true;
        else
            return false;
    }
}
