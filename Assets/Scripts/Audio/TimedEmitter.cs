using System.Collections;
using UnityEngine;

public class TimedEmitter : SoundEmitter
{
    [SerializeField]
    private int timer;

    private void Start()
    {
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSecondsRealtime(timer);
        AudioManager.instance.PlayEmitter(gameObject);
        yield return PlaySound();
    }
}
