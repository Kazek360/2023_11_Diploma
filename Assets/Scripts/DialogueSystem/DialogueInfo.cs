using Quests.ScriptableObjects;
using UnityEngine;

namespace DialogueSystem
{
    //Scriptable object storing information about dialogue.
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues/Dialogue")]
    public class DialogueInfo : ScriptableObject
    {
        public enum Type
        {
            Repeatable,
            QuestStarter,
            QuestProgression
        }
        
        [Header("General")] 
        [SerializeField] public Type dialogueType;
        
        [Header("Trigger")] 
        [SerializeField] public string triggerName;
        
        [Header("Quest")]
        [SerializeField] public QuestStepInfo unlockingStep;
        [SerializeField] public QuestInfo questToStart;
    
        [Header("Dialogue Text")]
        [SerializeField] public TextAsset dialogueText;
    }

}
