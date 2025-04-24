using Quests.ScriptableObjects;
using UnityEngine;

namespace Dialogues
{
    //Scriptable object storing information about dialogue.
    [CreateAssetMenu(fileName = "Dialogues", menuName = "ScriptableObjects/Dialogues/Dialogues")]
    public class DialogueInfo : ScriptableObject
    {
        public enum Type
        {
            Repeatable,
            Unrepeatable,
            QuestStarter,
            SideQuest,
            MainQuest
        }
        
        [Header("General")] 
        [SerializeField] public Type dialogueType;
        
        [Header("Trigger")] 
        [SerializeField] public string triggerName;
        
        [Header("Quest")]
        [SerializeField] public QuestInfo questToStart;
        
        [Header("Nestor")] 
        [SerializeField] public bool doesNestorAppear;
        [SerializeField] public string nestorID;
    
        [Header("Dialogues Text")]
        [SerializeField] public TextAsset dialogueText;
    }

}
