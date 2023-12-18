using System.Collections;
using TMPro;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    public string dialogueElementName;
    [SerializeField]
    private TextMeshPro textBox;
    [HideInInspector]
    public DialogueElement thisElement;
    private int currentLine = 0;
    [SerializeField]
    private GameObject prefab;

    private void Start()
    {
        if (prefab != null)
        {
            GameObject thisInstance = Instantiate(prefab, transform);

            if (!thisInstance.TryGetComponent(out Canvas canvas))
                return;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            if (!thisInstance.TryGetComponent(out textBox))
                return;
            textBox.alignment = TextAlignmentOptions.MidlineLeft;
        }
        PlaceHolderDialogueCall();
    }
    public void PlaceHolderDialogueCall()
    {
        Debug.Log(gameObject.name);
        StartCoroutine(CallLine());
    }

    private IEnumerator CallLine()
    {
        yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
        yield return WriteNextLine(thisElement.dialogueLines[currentLine]);
        currentLine++;
        if (currentLine >= thisElement.dialogueLines.Length)
        {
            if (prefab == null)
                StartCoroutine(GameManager.instance.LoadNextSceneAsync("TheGreenFieldScene"));
            currentLine = 0;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(CallLine());
        }
    }

    private IEnumerator WriteNextLine(string aLine)
    {
        Debug.Log(aLine);
        textBox.text = "";
        foreach (char aLetter in aLine)
        {
            textBox.text += aLetter;

            if (aLetter != '.' || aLetter != ',')
                yield return new WaitForSeconds(.05f);
            else
                yield return new WaitForSeconds(1f);
        }
    }
}
