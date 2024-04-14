using System.Collections.Generic;
using UnityEngine;

//Scriptable object storing information about dialogue.
[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues/Dialogue")]
public class DialogueSO : ScriptableObject
{
    [Header("Quest")]
    [SerializeField] public List<QuestSO> requiredQuests;
    [SerializeField] public QuestSO relatedQuest;
    [SerializeField] public bool startsQuest;
    
    [Header("Dialogue Text")]
    [SerializeField] public TextAsset dialogueText;
}
