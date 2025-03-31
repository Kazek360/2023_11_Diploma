using System.Collections.Generic;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    public class QuestInfo : ScriptableObject
    {
        [Header("General")]
        public string displayName;
        public QuestCategory questCategory;
        [TextArea] public string description;
        
        [Header("Rewards")] 
        public int experience;
        public List<string> items;
        
        [Header("First Step Transitions")]
        public List<QuestStepTransition> firstStepTransitions;
        
        [Header("Quests Transitions")]
        public List<QuestTransition> nextQuestsTransitions;
    }
}
