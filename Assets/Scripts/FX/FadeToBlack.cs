using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FadeToBlack : MonoBehaviour
{
    //Increases or decreases the duration of the fading effect. 
    [SerializeField]
    private float timeMultiplier = 1.0f;
    private Material _material;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }
    /// <summary>
    /// Fades the alpha of the fade out.
    /// </summary>
    /// <param name="fadeOut">Determines wether it should fade in or out</param>
    /// <returns></returns>
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
            elapsedTime += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        //Ensure the alpha is always either 0 or 1. 
        _material.SetFloat("_Alpha", endAlpha);
    }
}
