using System.Collections;
using TMPro;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    public string dialogueElementName;
    private TextMeshPro textBox;
    [HideInInspector]
    public DialogueElement thisElement;
    private int currentLine = 0;
    [SerializeField]
    private GameObject prefab;
    private Canvas canvas;
    IEnumerator runningNumerator;

    private void Start()
    {
        GameObject thisInstance = Instantiate(prefab, transform);

        if(!thisInstance.TryGetComponent(out canvas))
            return;
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        if (!thisInstance.TryGetComponent(out textBox))
            return;
        textBox.alignment = TextAlignmentOptions.MidlineLeft;

        canvas.enabled = false;
    }

    public void StartDialogue()
    {
        canvas.enabled = true;
        StartCoroutine(CallLine());
    }

    private IEnumerator CallLine()
    {
        if (runningNumerator != null)
            StopCoroutine(runningNumerator);
        yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
        yield return runningNumerator = WriteNextLine(thisElement.dialogueLines[currentLine]);
        currentLine++;
        if (currentLine >= thisElement.dialogueLines.Length)
        {
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
