using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using SpeechLib;

public class DialoguePlayer : MonoBehaviour
{
    public string dialogueElementName;
    private TextMeshPro textBox;
    private Canvas canvas;
    [HideInInspector]
    public DialogueElementJson thisElement;
    SpVoice voice = new SpVoice();
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
        StartCoroutine(WriteNextLine());
    }

    private IEnumerator WriteNextLine()
    {
        yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
        foreach (string aLine in thisElement.dialogueLines)
        {
            voice.Speak(aLine, SpeechVoiceSpeakFlags.SVSFlagsAsync | SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
            foreach (char aLetter in aLine)
            {
                textBox.text += aLetter;
                yield return new WaitForSeconds(.05f);
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
