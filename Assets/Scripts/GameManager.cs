using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public FadeToBlack fadeEffect;
	private string[] scenes = { "IntroScene", "TheGreenFieldScene", "EndingScene" };
	private static int sceneIndex;

	private void Awake()
	{
		if(instance == null)
			instance = this;
		else
			return;
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
		int nextScene = sceneIndex >= 2 ? 0 : ++sceneIndex;
		StartCoroutine(LoadNextSceneAsync(scenes[nextScene], true));
	}
	/// <summary>
	/// Loads another scene async and unloads the current scene using fade out
	/// </summary>
	/// <param name="scene">What scene to load</param>
	/// <param name="fadeOut">Checks if it should fade in</param>
	/// <returns></returns>
	public IEnumerator LoadNextSceneAsync(string scene, bool fadeOut)
	{
		Scene currentScene = SceneManager.GetActiveScene();
		yield return HandleFade(fadeOut);
		AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);
		yield return new WaitUntil(() => asyncOp.isDone == true);
		SceneManager.UnloadSceneAsync(currentScene);
	}
	/// <summary>
	/// Loads another scene async and unloads the current scene
	/// </summary>
	/// <param name="scene"></param>
	/// <returns></returns>
	public IEnumerator LoadNextSceneAsync(string scene)
	{
		Scene currentScene = SceneManager.GetActiveScene();
		AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);
		yield return new WaitUntil(() => asyncOp.isDone == true);
		SceneManager.UnloadSceneAsync(currentScene);
	}

	private IEnumerator HandleFade(bool fadeOut)
	{
		yield return fadeEffect.PlayEffect(fadeOut);
		yield return new WaitForSeconds(1.5f);
	}
}
