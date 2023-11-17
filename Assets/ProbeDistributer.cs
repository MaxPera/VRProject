using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SteamAudio;

public class ProbeDistributer : MonoBehaviour
{
    [SerializeField]
    private List<ProbeMatches> matches = new List<ProbeMatches>();

    private void Awake()
    {
        PickMatch();
    }

    void PickMatch()
    {
        foreach (ProbeMatches aMatch in matches)
            foreach (SteamAudioPreset aPreset in aMatch.audioPresets)
                aPreset.probeBatch = aMatch.probeBatch;
    }
}
[System.Serializable]
public struct ProbeMatches
{
    public SteamAudioProbeBatch probeBatch;
    public SteamAudioPreset[] audioPresets;
}