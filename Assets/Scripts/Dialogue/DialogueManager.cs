using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance;

	public TextAsset[] DialogueFiles;

	[HideInInspector] public static TextAsset ChosenDialogueFile;
	[HideInInspector] public static DialogueElements DialogueElementsList;

	private DialoguePlayer[] dialoguePlayers;
	[field: Header("Json string that defines on completion of task")]
	public string onComplete;

	private void Awake()
	{
		//Checks if there is no instance in which case it creates a new one
		if(Instance == null) {
			Instance = this;
		}
		//If there is an instance save it
		else
			Destroy(this);

		dialoguePlayers = FindObjectsOfType<DialoguePlayer>();
	}
	private void Start()
	{
		if(ChosenDialogueFile == null)
			ChosenDialogueFile = DialogueFiles[0];

		string jsonData = ChosenDialogueFile.text;
		ParseJson(jsonData);
	}

	private void ParseJson(string jsonData)
	{
		DialogueElementsList = JsonUtility.FromJson<DialogueElements>(jsonData);
		StartCoroutine(AssignDialogue(DialogueElementsList));
	}
	/// <summary>
	/// Switches the language of the dialogue.
	/// </summary>
	/// <param name="text">What Json to parse</param>
	public void SwitchLanguage(TMP_Text text)
	{
		int nextLanguageIndex = Array.IndexOf(DialogueFiles, ChosenDialogueFile) + 1;
		nextLanguageIndex = nextLanguageIndex > DialogueFiles.Length - 1 ? 0 : nextLanguageIndex;
		ChosenDialogueFile = DialogueFiles[nextLanguageIndex];
		text.text = ChosenDialogueFile.name;
		ParseJson(ChosenDialogueFile.text);
	}
	/// <summary>
	/// Assigns all the dialogue to the correct player
	/// </summary>
	/// <param name="dialogueElements">List of all the dialogueElements from the Json parse</param>
	/// <returns></returns>
	private IEnumerator AssignDialogue(DialogueElements dialogueElements)
	{
		yield return new WaitUntil(() => DialogueElementsList != null);
		yield return new WaitUntil(() => dialoguePlayers != null);
		foreach(DialoguePlayer aPlayer in dialoguePlayers) {
			foreach(DialogueElement anElement in dialogueElements.dialogueElements) {
				if(aPlayer.dialogueElementName == anElement.dialogueName) {
					aPlayer.thisElement = anElement;
				} else
					yield return null;
			}
		}
	}
}
