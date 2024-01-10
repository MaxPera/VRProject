using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    private float _fadeTime = 5.0f;
    //Increases or decreases the duration of the fading effect. 
    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float timeScale = 1.0f;
    private Material _material;
    private bool _isFadingOut = false;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        Fade(true);
    }
    /// <summary>
    /// Fade the players view to black or fade-in from black. 
    /// </summary>
    /// <param name="fadeOut">Set true to fade to black, false to fade in.</param>
    void Fade(bool fadeOut)
    {
        //If we've already faded to black, we cannot fade out again. 
        if (fadeOut && _isFadingOut)
            return;

        _isFadingOut = fadeOut;
        StopAllCoroutines();
        StartCoroutine(PlayEffect(fadeOut));
    }

    private IEnumerator PlayEffect(bool fadeOut)
    {
        float startAlpha = _material.GetFloat("_Alpha");
        float endAlpha = fadeOut ? 1.0f : 0.0f;

        float elapsedTime = 0.0f;
        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime * timeScale;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime);
            _material.SetFloat("_Alpha", alpha);
            yield return null;
        }
        //Ensure the alpha is always either 0 or 1. 
        _material.SetFloat("_Alpha", endAlpha);
    }
}
