using System;
using System.Collections.Generic;
using Quests.Flags;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Quests/Quest")]
    public class QuestInfo : ScriptableObject
    {
        [Header("General")]
        public string displayName;
        public QuestCategory questCategory;
        [TextArea] public string description;
        
        [Header("Requirements")]
        public List<FlagInfo> requiredFlags;
        
        [Header("Quests")]
        public List<QuestInfo> requiredQuests;
        public List<QuestInfo> nextQuests;

        [Header("Steps")]
        public List<GameObject> steps;

        [Header("Rewards")] 
        public int experience;
        public List<int> items;
    }
}
