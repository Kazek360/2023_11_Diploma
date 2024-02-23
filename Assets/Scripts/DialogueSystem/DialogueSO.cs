using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues2/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string id;
    
    [Header("General")]
    [SerializeField] public int priority;
    
    [Header("Quest")]
    [SerializeField] public QuestSO quest;
    [SerializeField] public DialogueQuestProgress questProgress;
    
    [Header("Requirements & sentences")]
    [SerializeField] public List<QuestSO> requirements;
    [SerializeField] public List<SentenceSO> sentences;
    
    private void OnValidate()
    {
        #if UNITY_EDITOR
            id = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

}
