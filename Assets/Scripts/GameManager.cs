using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public FadeToBlack fadeEffect;
    private bool hasFaded = false;
    private string[] scenes = { "TheGreenFieldScene", "EndingScene", "TextScene" };
    private static int nextScene;
    private static int sceneIndex;

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
        EventBus.Instance.Subscribe<FadeInEvent>(fadeInOnload);
        EventBus.Instance.SendEvent(this, new FadeInEvent());
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void FadeOutAndLoadScene()
    {
        EventBus.Instance.SendEvent(this, new FadeOutEvent());
    }

    private void fadeInOnload(object sender, EventArgs args)
    {
        StartCoroutine(HandleFade(false));
    }

    private void HandleLoad(object sender, EventArgs args)
    {
        nextScene = sceneIndex >= 2 ? 0 : ++sceneIndex;
        Debug.Log(nextScene);
        StartCoroutine(LoadNextSceneAsync(scenes[nextScene], true));
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
        yield return fadeEffect.PlayEffect(fadeOut);
        hasFaded = fadeOut;
        yield return new WaitForSeconds(1.5f);
    }
}
