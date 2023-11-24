using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Volume postProcessing;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            return;

        if (postProcessing == null)
            postProcessing = FindFirstObjectByType<Volume>();
        else
            return;

        DontDestroyOnLoad(instance);
        DontDestroyOnLoad(postProcessing);
    }
}
