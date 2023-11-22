using SteamAudio;
using System.Collections.Generic;
using UnityEngine;

public class ProbeDistributer : MonoBehaviour
{
    [SerializeField]
    private List<ProbeMatches> matches = new();

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