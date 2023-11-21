using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ResearchAudioPlayer : MonoBehaviour
{
    public Transform thisTransform;
    public EventReference eventReference, pauseSound;
    public List<AudioRound<Vector3, Vector3>> audioRounds = new List<AudioRound<Vector3, Vector3>>();
    private List<AudioRound<Vector3, Vector3>> usedRound = new List<AudioRound<Vector3, Vector3>>();
    AudioRound<Vector3, Vector3> thisRound;
    public List<AudioRound<AudioRound<Vector3, Vector3>, Vector3>> pickList = new List<AudioRound<AudioRound<Vector3, Vector3>, Vector3>>();
    int counter = 0;

    public static ResearchAudioPlayer instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        thisRound = audioRounds[Random.Range(0, audioRounds.Count - 1)];
        yield return PlaySound(thisRound);
        counter++;
        yield return new WaitForSeconds(2);
        StartCoroutine(StartRound());
    }
    public IEnumerator PlaySound(AudioRound<Vector3,Vector3> thisRound)
    {
        yield return new WaitForSeconds(4);
        AudioManager.instance.PlayOneShot(eventReference, thisRound.Value);
        Debug.Log(thisRound.Value);
        yield return new WaitForSeconds(4);
        AudioManager.instance.PlayOneShot(eventReference, thisRound.Key);
        Debug.Log(thisRound.Key);
        yield return new WaitForSeconds(4);
        AudioManager.instance.PlayOneShot(eventReference, thisRound.Value);
        yield return new WaitForSeconds(4);
        AudioManager.instance.PlayOneShot(eventReference, thisRound.Key);
        audioRounds.Remove(thisRound);
        usedRound.Add(thisRound);
        yield return null;
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
