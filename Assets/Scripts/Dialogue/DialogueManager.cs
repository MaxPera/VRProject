using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	public static DialogueManager Instance;

	public TextAsset[] DialogueFiles;

	[HideInInspector] public TextAsset ChosenDialogueFile;
	[HideInInspector] public DialogueElements DialogueElementsList;

	private DialoguePlayer[] dialoguePlayers;

	private void Awake()
	{
		//Checks if there is no instance in which case it creates a new one
		if(Instance == null) {
			Instance = this;
		}
		//If there is an instance save it
		else
			Destroy(this);

		DontDestroyOnLoad(Instance);

		dialoguePlayers = FindObjectsOfType<DialoguePlayer>();
	}
	private void Start()
	{
		string jsonData = ChosenDialogueFile.text;
		ParseJson(jsonData);
	}

	private void ParseJson(string jsonData)
	{
		DialogueElementsList = JsonUtility.FromJson<DialogueElements>(jsonData);
		StartCoroutine(AssignDialogue(DialogueElementsList));
	}

	public void SwitchLanguage(TMP_Text text)
	{
		int nextLanguageIndex = Array.IndexOf(DialogueFiles, ChosenDialogueFile) + 1;
		nextLanguageIndex = nextLanguageIndex > DialogueFiles.Length - 1 ? 0 : nextLanguageIndex;
		ChosenDialogueFile = DialogueFiles[nextLanguageIndex];
		text.text = ChosenDialogueFile.name;
	}

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
