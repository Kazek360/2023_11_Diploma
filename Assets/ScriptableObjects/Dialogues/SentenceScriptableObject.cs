using System.Collections.Generic;
using UnityEngine;

/**
 * A scriptable object containing one part of a dialogue along with options for eventual choice
 */
[CreateAssetMenu(fileName = "SentenceScriptableObject", menuName = "ScriptableObjects/Dialogues/Sentence")]
public class SentenceScriptableObject : ScriptableObject
{
    [SerializeField] private string speakerName;    //Name of the character speaking
    [SerializeField] private string text;   //Text that should appear on screen
    [SerializeField] private string flag;   //(OPTIONAL) A flag that should be switched by this sentence appearing
    [SerializeField] private Sprite speakerPortrait;    //Portrait of the speaker
    
    
    [SerializeField] private bool isChoice; //Should this sentence present player with a choice
    [SerializeField] private string choiceA;    //Text of choice A
    [SerializeField] private string choiceB;    //Text of choice B
    [SerializeField] private DialogueScriptableObject dialogueA;    //Dialogue that should appear after choosing option A
    [SerializeField] private DialogueScriptableObject dialogueB;    //Dialogue that should appear after choosing option B

    public string SpeakerName
    {
        get => speakerName;
        set => speakerName = value;
    }

    public string Text
    {
        get => text;
        set => text = value;
    }

    public string Flag
    {
        get => flag;
        set => flag = value;
    }

    public Sprite SpeakerPortrait
    {
        get => speakerPortrait;
        set => speakerPortrait = value;
    }

    public bool IsChoice
    {
        get => isChoice;
        set => isChoice = value;
    }

    public string ChoiceA
    {
        get => choiceA;
        set => choiceA = value;
    }

    public string ChoiceB
    {
        get => choiceB;
        set => choiceB = value;
    }

    public DialogueScriptableObject DialogueA
    {
        get => dialogueA;
        set => dialogueA = value;
    }

    public DialogueScriptableObject DialogueB
    {
        get => dialogueB;
        set => dialogueB = value;
    }
}