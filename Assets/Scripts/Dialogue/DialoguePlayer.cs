using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class DialoguePlayer : MonoBehaviour
{
    public string dialogueElementName;
    private TextMeshPro textBox;
    private Canvas canvas;
    [HideInInspector]
    public DialogueElementJson thisElement;
    private int currentLine = 0;
    private void Start()
    {
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
        }
        else
        {
            return;
        }
        if (textBox == null)
        {
            textBox = gameObject.AddComponent<TextMeshPro>();
            textBox.alignment = TextAlignmentOptions.MidlineLeft;
        }
        else
        {
            return;
        }
        
    }

    private IEnumerator CallLine()
    {
        yield return WriteNextLine(thisElement.dialogueLines[currentLine]);
        currentLine++;
    }

    private IEnumerator WriteNextLine(string aLine)
    {
        yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
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
