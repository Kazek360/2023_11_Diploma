using Quests.ScriptableObjects;
using UnityEngine;

namespace DialogueSystem
{
    //Scriptable object storing information about dialogue.
    [CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        [Header("Trigger")] 
        [SerializeField] public string triggerName;
    
        [Header("Quest")]
        [SerializeField] public QuestInfo relatedQuest;
        [SerializeField] public bool startsQuest;
    
        [Header("Dialogue Text")]
        [SerializeField] public TextAsset dialogueText;
    }

}
