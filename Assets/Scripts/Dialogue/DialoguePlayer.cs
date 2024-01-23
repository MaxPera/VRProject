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

		if(_playOnStart) {
			StartDialogue();
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
			animatorScript.talkingBool = false;
		}
		//Checks if dialogueLine has Json code
		else if (thisElement.dialogueLines[currentLine].Contains(DialogueManager.Instance.onComplete))
		{
			yield return new WaitUntil(() => onComplete == true);
			foreach (char aChar in DialogueManager.Instance.onComplete)
            {
				thisElement.dialogueLines[currentLine].Trim(aChar);
            }
			StartCoroutine(CallLine());
		}
		else
		{
			yield return new WaitForSeconds(2f);
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
