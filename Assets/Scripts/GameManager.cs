using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public FadeToBlack fadeEffect; 
    private bool hasFaded = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;

        DontDestroyOnLoad(instance);
    }

    private void Start()
    {
        EventBus.Instance.Subscribe<FadeOutEvent>(HandleLoad);
        EventBus.Instance.SendEvent(this, new FadeOutEvent());
    }

    public void StartFade() 
    {
        EventBus.Instance.SendEvent(this, new FadeOutEvent()); 
    }

    private void HandleLoad(object sender, EventArgs args) 
    {
        StartCoroutine(LoadNextSceneAsync("EndingScene", true));      
    }

    public IEnumerator LoadNextSceneAsync(string scene, bool fadeOut)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        yield return HandleFade(fadeOut);
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);
        yield return new WaitUntil(() => asyncOp.isDone == true);
        SceneManager.UnloadSceneAsync(currentScene);

    }

    private IEnumerator HandleFade(bool fadeOut) 
    {
        if (hasFaded && fadeOut) yield return null;
        //if (!hasFaded && !fadeOut) yield return null;
        yield return fadeEffect.PlayEffect(fadeOut);
        hasFaded = fadeOut;
        yield return new WaitForSeconds(1.5f); 
    }
}
