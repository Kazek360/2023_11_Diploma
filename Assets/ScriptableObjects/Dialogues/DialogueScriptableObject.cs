using System.Collections.Generic;
using UnityEngine;

/**
 * A scriptable object containing an entire dialogue to be used when talking with an NPC
 */
[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "ScriptableObjects/DialoguesOld/Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    [SerializeField] private List<SentenceScriptableObject> sentences;
    [SerializeField] private int priority;  //When multiple dialogues are available, priority decides which will be displayed
    [SerializeField] private List<string> requiredFlags;    //What flags are necessary for this dialogue to be available

    public int GetDialogueLength()
    {
        return sentences.Capacity;
    }

    public SentenceScriptableObject GetSentence(int i)
    {
        return sentences[i];
    }

    public List<string> RequiredFlags => requiredFlags;

    public int Priority => priority;
}