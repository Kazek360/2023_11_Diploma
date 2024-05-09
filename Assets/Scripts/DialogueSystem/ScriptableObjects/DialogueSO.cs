using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.ScriptableObjects
{
    //Scriptable object storing information about dialogue.
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [Header("Trigger")] 
        [SerializeField] public string triggerName;
    
        [Header("Quest")]
        [SerializeField] public QuestSO relatedQuest;
        [SerializeField] public bool startsQuest;
    
        [Header("Dialogue Text")]
        [SerializeField] public TextAsset dialogueText;
    }

}
