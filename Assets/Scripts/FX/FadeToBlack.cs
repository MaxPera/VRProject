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
    private float timeMultiplier = 1.0f;
    private Material _material;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }
    public IEnumerator PlayEffect(bool fadeOut)
    {
        float startAlpha = _material.GetFloat("_Alpha");
        float endAlpha = fadeOut ? 1.0f : 0.0f;
        float elapsedTime = 0.0f;
        float alpha = startAlpha; 

        while (alpha != endAlpha)
        {           
            alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime);
            _material.SetFloat("_Alpha", alpha);
            //Debug.Log(alpha);
            elapsedTime += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        //Ensure the alpha is always either 0 or 1. 
        _material.SetFloat("_Alpha", endAlpha);
    }
}
