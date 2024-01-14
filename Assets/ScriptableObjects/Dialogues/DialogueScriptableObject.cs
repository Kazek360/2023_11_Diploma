using System.Collections.Generic;
using UnityEngine;

/**
 * A scriptable object containing an entire dialogue to be used when talking with an NPC
 */
[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "ScriptableObjects/Dialogues/Dialogue")]
public class DialogueScriptableObject : ScriptableObject
{
    [SerializeField] private List<SentenceScriptableObject> sentences;

    public int GetDialogueLength()
    {
        return sentences.Capacity;
    }

    public SentenceScriptableObject GetSentence(int i)
    {
        return sentences[i];
    }

}