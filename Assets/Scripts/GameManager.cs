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
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(scene);
        yield return new WaitUntil(() => asyncOp.isDone == true);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

    }

}
