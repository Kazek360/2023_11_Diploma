using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sentence", menuName = "ScriptableObjects/Dialogues2/Sentence")]
public class SentenceSO : ScriptableObject
{
    [SerializeField] private Sprite portrait;
    [SerializeField] private string speakerName;    
    [TextArea]
    [SerializeField] private string text;   
        
    
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

    public Sprite Portrait
    {
        get => portrait;
        set => portrait = value;
    }
}
