using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    public string dialogueElementName;
    private TextMeshPro textBox;
    private Canvas canvas;
    public DialogueElementXML thisElement {private get;  set; }
    private void Start()
    {
        canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        textBox = gameObject.AddComponent<TextMeshPro>();
        StartCoroutine(WriteNextLine());
    }

    private IEnumerator WriteNextLine()
    {
        yield return new WaitUntil(() => thisElement != null);

        foreach (string aLine in thisElement.dialogueLines)
        {
            if (thisElement.dialogueLines.Count != 1)
                textBox.text += $"{aLine}, ";
            else
                textBox.text += $"{aLine}.";    
            Debug.Log(textBox.text);
            yield return new WaitForSeconds(.5f);
        }
    }
}
