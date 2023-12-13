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
    public DialogueElements dialogueElementsJsonList = new DialogueElements();


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
        dialogueElementsJsonList = JsonUtility.FromJson<DialogueElements>(jsonData);
        StartCoroutine(AssignDialogue(dialogueElementsJsonList));
    }

    private IEnumerator AssignDialogue(DialogueElements dialogueElements)
    {
        yield return new WaitUntil(() => dialogueElementsJsonList != null);
        yield return new WaitUntil(() => dialoguePlayers != null);
        foreach (DialoguePlayer aPlayer in dialoguePlayers)
        {
            foreach (DialogueElement anElement in dialogueElements.dialogueElements)
            {
                if (aPlayer.dialogueElementName == anElement.dialogueName)
                {
                    aPlayer.thisElement = anElement;
                }
                else
                    yield return null;
            }
        }
    }
}
