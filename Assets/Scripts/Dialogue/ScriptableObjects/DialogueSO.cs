using System.Collections.Generic;
using UnityEngine;

//Scriptable object storing information about dialogue.
[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public string id;
    
    [Header("Requirements")]
    [SerializeField] public DialogueMarkerSO requiredFlag;
    [SerializeField] public List<QuestSO> requiredQuests;
    
    [Header("Quest")]
    [SerializeField] public QuestSO relatedQuest;
    [SerializeField] public DialogueQuestProgress questProgress;
    
    [Header("Starting sentence")]
    [SerializeField] public SentenceSO firstSentence;
     
    private void OnValidate()
    {
        #if UNITY_EDITOR
            id = name;
            UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
