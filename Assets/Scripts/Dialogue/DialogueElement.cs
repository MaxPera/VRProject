using System;

[Serializable]
public class DialogueElement
{
    public string dialogueName;
    public string[] dialogueLines;
}

[Serializable]
public class DialogueElements
{
    public DialogueElement[] dialogueElements;
}
