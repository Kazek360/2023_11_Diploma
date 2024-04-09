using System.Collections.Generic;
using UnityEngine;

// A scriptable object storing information about sentence in dialogue
[CreateAssetMenu(fileName = "Sentence", menuName = "ScriptableObjects/Dialogues/Sentence")]
public class SentenceSO : ScriptableObject
{
    [Header("Sentence info")]
    [SerializeField] public Sprite portrait;
    [SerializeField] public string speakerName;    
    [TextArea]
    [SerializeField] public string text;
    
    [Header("Flags")]
    [SerializeField] public FlagSO flagToTrigger;
    
    [Header("Next Sentences")] 
    [SerializeField] public List<SentenceSO> nextSentences;
    [TextArea][SerializeField] public List<string> answers;
}
