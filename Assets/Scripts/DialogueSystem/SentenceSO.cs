using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentence", menuName = "ScriptableObjects/Dialogues2/Sentence")]
public class SentenceSO : ScriptableObject
{
    [SerializeField] private string speakerName;    //Name of the character speaking
    [TextArea]
    [SerializeField] private string text;   //Text that should appear on screen
    [SerializeField] private List<string> flags;   //(OPTIONAL) List of flags that should be set with appearance of this sentence
    [SerializeField] private Sprite speakerPortrait;    //Portrait of the speaker
    
    
    [SerializeField] private bool isChoice; //Should this sentence present player with a choice
    [SerializeField] private string choiceA;    //Text of choice A
    [SerializeField] private string choiceB;    //Text of choice B
    [SerializeField] private DialogueSO dialogueA;    //Dialogue that should appear after choosing option A
    [SerializeField] private DialogueSO dialogueB;    //Dialogue that should appear after choosing option B

    public List<string> Flags => flags;

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

    public DialogueSO DialogueA
    {
        get => dialogueA;
        set => dialogueA = value;
    }

    public DialogueSO DialogueB
    {
        get => dialogueB;
        set => dialogueB = value;
    }
}
