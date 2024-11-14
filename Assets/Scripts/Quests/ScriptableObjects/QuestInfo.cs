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
        
        [Header("Flag Requirements")]
        public List<FlagInfo> requiredFlags;
        
        [Header("Quests")]
        public List<QuestInfo> requiredQuests;
        public List<QuestInfo> nextQuests;

        [Header("Steps")]
        public QuestStepInfo firstStep;

        [Header("Rewards")] 
        public int experience;
        public List<int> items;
    }
}
