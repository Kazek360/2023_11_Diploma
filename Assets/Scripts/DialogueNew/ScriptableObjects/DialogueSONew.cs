using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object storing information about dialogue.
[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues/Dialogue")]
public class DialogueSONew : ScriptableObject
{
    public string id;
    
    [Header("General")]
    [SerializeField] public int priority;
    
    [Header("Quests")]
    [SerializeField] public List<QuestSO> requiredQuests;
    [SerializeField] public QuestSO relatedQuest;
    [SerializeField] public DialogueQuestProgressNew questProgress;
    
    [Header("Starting sentence")]
    [SerializeField] public SentenceSONew firstSentence;
     
    private void OnValidate()
    {
        #if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

}
