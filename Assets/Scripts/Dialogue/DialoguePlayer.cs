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
	[SerializeField]
	private bool _playOnStart;
	private Canvas canvas;
	IEnumerator runningNumerator;
	public bool onComplete;
	public bool onValve;
	public bool onBalloon;
	public bool onMaze;

    private void Start()
	{
		GameObject thisInstance = Instantiate(prefab, transform);

		if(!thisInstance.TryGetComponent(out canvas))
			return;
		canvas.renderMode = RenderMode.WorldSpace;
		canvas.worldCamera = Camera.main;


		if(!(textbox = thisInstance.GetComponentInChildren<TextMeshProUGUI>()))
			return;
		textbox.alignment = TextAlignmentOptions.MidlineLeft;

		canvas.enabled = false;

		if(onValve)
			EventBus.Instance.Subscribe<ValveTurnedEvent>(SetOnValveComplete);
        if (onBalloon)
            EventBus.Instance.Subscribe<BalloonEvent>(SetOnBalloonComplete);
        if (onMaze)
            EventBus.Instance.Subscribe<MazeCompletedEvent>(SetOnMazeComplete);

        if (_playOnStart) {
			StartDialogue();
		}
	}

    public void SetOnValveComplete(object sender = null, EventArgs args = null)
    {
		onComplete = true;
    }

    public void SetOnBalloonComplete(object sender = null, EventArgs args = null)
    {
        onComplete = true;
    }

    public void SetOnMazeComplete(object sender = null, EventArgs args = null)
    {
        onComplete = true;
    }

    public void SetOnComplete()
    {
        onComplete = true;
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
	/// <summary>
	/// Writes all the lines in the dialogue
	/// </summary>
	/// <returns></returns>
	private IEnumerator CallLine()
	{
		//Checks if there is another dialogue running, if so it stops it
		if(runningNumerator != null)
			StopCoroutine(runningNumerator);
		if(transform.parent.TryGetComponent(out AnimatorScript animatorScript))
			animatorScript.talkingBool = true;
		//Waits for all the dialogue.
		yield return new WaitUntil(() => thisElement.dialogueLines.Length > 0);
		yield return runningNumerator = WriteNextLine(thisElement.dialogueLines[currentLine]);

		currentLine++;
		//Checks if dialogue has reached end
		if (currentLine >= thisElement.dialogueLines.Length)
		{
			currentLine = 0;
			if(animatorScript != null)
				animatorScript.talkingBool = false;
			Debug.Log("Dialogue done");
		}
		//Checks if dialogueLine has Json code
		else if (thisElement.dialogueLines[currentLine].Contains(DialogueManager.onComplete))
		{
            if (animatorScript != null)
                animatorScript.talkingBool = false;
            yield return new WaitUntil(() => onComplete == true);
            if (animatorScript != null)
                animatorScript.talkingBool = true;
            thisElement.dialogueLines[currentLine] = thisElement.dialogueLines[currentLine].Substring(DialogueManager.onComplete.Length, thisElement.dialogueLines[currentLine].Length - DialogueManager.onComplete.Length);
            Debug.Log("Confirmation");
            StartCoroutine(CallLine());
		}
		else
		{
			yield return new WaitForSeconds(2f);
            Debug.Log("Starting next panel");
            StartCoroutine(CallLine());
		}
	}

	/// <summary>
	/// Writes every line per character
	/// </summary>
	/// <param name="aLine">What line to write</param>
	/// <returns></returns>
	private IEnumerator WriteNextLine(string aLine)
	{
		textbox.text = "";
		foreach(char aLetter in aLine) {
			textbox.text += aLetter;

			if(aLetter != '.' || aLetter != ',')
				yield return new WaitForSeconds(.05f);
			else
				yield return new WaitForSeconds(1f);
		}
	}
}
