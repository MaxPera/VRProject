using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;/*
    [HideInInspector]
    public List<DialogueElementXML> dialogueElementsXml = new List<DialogueElementXML>();*/
    [HideInInspector]
    public DialogueElementsJson dialogueElementsJson = new DialogueElementsJson();
/*
    [SerializeField]
    private TextAsset xmlRawFile;*/
    [SerializeField]
    private TextAsset jsonRawFile;

    [SerializeField]
    private DialoguePlayer[] dialoguePlayers;

    private void Awake()
    {
        //Checks if there is no instance in which case it creates a new one
        if (instance == null)
        {
            instance = this;
        }
        //If there is an instance save it
        else
            Destroy(this);

        DontDestroyOnLoad(instance);
/*
        string xmlData = xmlRawFile.text;
        ParseXML(xmlData);*/

        string jsonData = jsonRawFile.text;
        Debug.Log(jsonData);
        ParseJson(jsonData);

        dialoguePlayers = FindObjectsOfType<DialoguePlayer>();
    }

    private void OnDisable()
    {
        CleanUpDialogue();
    }/*

    private void ParseXML(string xmlData)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(new StringReader(xmlData));

        foreach (DialoguePlayer anElement in dialoguePlayers)
        {
            XmlNodeList thisList = xmlDoc.SelectNodes($"//data/{anElement.dialogueElementName}");
            foreach (XmlNode aNode in thisList)
            {
                anElement.thisElement = GetDialogueXML(aNode);
            }
        }
    }*/

    private void ParseJson(string jsonData)
    {
        dialogueElementsJson = JsonUtility.FromJson<DialogueElementsJson>(jsonData);
        Debug.Log(dialogueElementsJson);
    }/*

    private DialogueElementXML GetDialogueXML(XmlNode xmlNode)
    {
        XmlNode name = xmlNode;

        DialogueElementXML dialogueElement = ScriptableObject.CreateInstance<DialogueElementXML>();
        dialogueElement.name = name.Name;
        dialogueElement.dialogueName = name.Name;
        Debug.Log(dialogueElement.dialogueName);
        foreach (XmlNode dialogueNode in name)
        {
            dialogueElement.dialogueLines.Add(dialogueNode.Name);
            Debug.Log(dialogueNode.InnerText);
        }
        dialogueElementsXml.Add(dialogueElement);
        return dialogueElement;
    }*/

    private void CleanUpDialogue()
    {/*
        if (dialogueElementsXml.Count != 0)
        {
            dialogueElementsXml.Clear();
        }*/
    }

    private void AssignDialogue()
    {

    }
}
