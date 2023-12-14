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

    private void Start()
    {
        GameObject thisInstance = Instantiate(prefab, transform);

        if (!thisInstance.TryGetComponent(out Canvas canvas))
            return;
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        if (!thisInstance.TryGetComponent(out textBox))
            return;
        textBox.alignment = TextAlignmentOptions.MidlineLeft;

        StartCoroutine(CallLine());
    }

    private IEnumerator CallLine()
    {
        yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
        yield return WriteNextLine(thisElement.dialogueLines[currentLine]);
        currentLine++;
    }

    private IEnumerator WriteNextLine(string aLine)
    {
        foreach (char aLetter in aLine)
        {
            textBox.text += aLetter;

            if (aLetter != '.' || aLetter != ',')
                yield return new WaitForSeconds(.05f);
            else
                yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator WriteNextLine(string aLine, float clipLength)
    {
        float waitForLetter = clipLength / aLine.ToCharArray().Length;
        foreach (char aLetter in aLine)
        {
            textBox.text += aLetter;
            yield return new WaitForSeconds(waitForLetter);
        }
    }

    private IEnumerator WriteNextLine(string aLine, float clipLength, bool conditionIsMet)
    {
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
