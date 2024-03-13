using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scriptable object storing information about dialogue.
[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string id;
    
    [Header("General")]
    [SerializeField] public int priority;
    
    [Header("Quests")]
    [SerializeField] public List<QuestSO> requiredQuests;
    [SerializeField] public QuestSO relatedQuest;
    [SerializeField] public DialogueQuestProgress questProgress;
    
    [Header("Starting sentence")]
    [SerializeField] public SentenceSO firstSentence;
     
    private void OnValidate()
    {
        #if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

}
