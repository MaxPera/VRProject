using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [SerializeField]
    private TextAsset jsonRawFile;

    [SerializeField]
    private DialoguePlayer[] dialoguePlayers;

    [HideInInspector]
    public DialogueElementsJson dialogueElementsJsonList = new DialogueElementsJson();


    private void Awake()
    {
        //Checks if there is no instance in which case it creates a new one
        if (instance == null)
        {
            instance = this;
        }
        //If there is an instance save it
        else
            Destroy(this);

        DontDestroyOnLoad(instance);

        dialoguePlayers = FindObjectsOfType<DialoguePlayer>();
    }
    private void Start()
    {
        string jsonData = jsonRawFile.text;
        ParseJson(jsonData);
    }

    private void ParseJson(string jsonData)
    {
        dialogueElementsJsonList = JsonUtility.FromJson<DialogueElementsJson>(jsonData);
        StartCoroutine(AssignDialogue(dialogueElementsJsonList));
    }

    private IEnumerator AssignDialogue(DialogueElementsJson dialogueElements)
    {
        yield return new WaitUntil(() => dialogueElementsJsonList != null);
        yield return new WaitUntil(() => dialoguePlayers != null);
        foreach (DialoguePlayer aPlayer in dialoguePlayers)
        {
            foreach (DialogueElementJson anElement in dialogueElements.dialogueElementsJson)
            {
                if (aPlayer.dialogueElementName == anElement.dialogueName)
                {
                    Debug.Log(aPlayer);
                    aPlayer.thisElement = anElement;
                }
                else
                    yield return null;
            }
        }
    }
}
