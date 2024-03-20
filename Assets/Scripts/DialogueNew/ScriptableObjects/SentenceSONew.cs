using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A scriptable object storing information about sentence in dialogue
[CreateAssetMenu(fileName = "Sentence", menuName = "ScriptableObjects/Dialogues/Sentence")]
public class SentenceSONew : ScriptableObject
{
    [SerializeField] public Sprite portrait;
    [SerializeField] public string speakerName;    
    [TextArea]
    [SerializeField] public string text;

    [Header("Sentences")] 
    [SerializeField] public List<SentenceSONew> nextSentences;
    [TextArea][SerializeField] public List<string> answers;
}
