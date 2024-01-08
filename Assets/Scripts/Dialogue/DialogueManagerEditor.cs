using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DialogueManager))]
public class DialogueManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		DialogueManager dialogueManager = (DialogueManager)target;

		if(dialogueManager.DialogueFiles.Length > 0) {
			if(dialogueManager.ChosenDialogueFile == null) {
				dialogueManager.ChosenDialogueFile = dialogueManager.DialogueFiles[0];
			}

			string[] dialogueFileNames = Array.ConvertAll(dialogueManager.DialogueFiles, file => file.name);

			int chosenIndex = EditorGUILayout.Popup("Select language:", Array.IndexOf(dialogueFileNames, dialogueManager.ChosenDialogueFile.name), dialogueFileNames);
			dialogueManager.ChosenDialogueFile = dialogueManager.DialogueFiles[chosenIndex];
		}
	}
}
#endif
