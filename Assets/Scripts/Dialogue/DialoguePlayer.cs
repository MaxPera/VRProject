using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialoguePlayer : MonoBehaviour
{
    public string dialogueElementName;
    private TextMeshProUGUI textbox;
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

        
        if (!(textbox = thisInstance.GetComponentInChildren<TextMeshProUGUI>()))
            return;
        textbox.alignment = TextAlignmentOptions.MidlineLeft;
        
        canvas.enabled = false;
        
        if (dialogueElementName == "Hay1")
        {
            canvas.enabled = true;
            StartCoroutine(CallLine());
        }
        
    }

    public void StartDialogue()
    {
        canvas.enabled = true;
        StartCoroutine(CallLine());
    }
    public void EndDialogue()
    {
        canvas.enabled = false;
        textbox.text = "";
        
    }
    private IEnumerator CallLine()
    {
        if (runningNumerator != null)
            StopCoroutine(runningNumerator);
        yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
        yield return runningNumerator = WriteNextLine(thisElement.dialogueLines[currentLine]);
        currentLine++;
        if (transform.parent.TryGetComponent(out AnimatorScript animatorScript))
            animatorScript.talkingBool = true;
        if (currentLine >= thisElement.dialogueLines.Length)
        {
            currentLine = 0;
            animatorScript.talkingBool = false;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(CallLine());
        }
    }

    private IEnumerator WriteNextLine(string aLine)
    {
        textbox.text = "";
        foreach (char aLetter in aLine)
        {
            textbox.text += aLetter;

            if (aLetter != '.' || aLetter != ',')
                yield return new WaitForSeconds(.05f);
            else
                yield return new WaitForSeconds(1f);
        }
    }
}
