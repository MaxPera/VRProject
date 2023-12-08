using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class DialoguePlayer : MonoBehaviour
{
    public string dialogueElementName;
    private TextMeshPro textBox;
    private Canvas canvas;
    public DialogueElementJson thisElement;
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
        }
        else
        {
            return;
        }
        StartCoroutine(WriteNextLine());
    }

    private IEnumerator WriteNextLine()
    {
        yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
        foreach (string aLine in thisElement.dialogueLines)
        {
            textBox.text += aLine;
            Debug.Log(textBox.text);
            yield return new WaitForSeconds(.5f);
        }
    }
}
