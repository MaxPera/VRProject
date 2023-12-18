using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;

        DontDestroyOnLoad(instance);
    }

    public IEnumerator LoadNextSceneAsync(string scene)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);
        Debug.Log(asyncOp.progress);
        yield return new WaitUntil(() => asyncOp.isDone == true);
        SceneManager.UnloadSceneAsync(currentScene);

    }

}
