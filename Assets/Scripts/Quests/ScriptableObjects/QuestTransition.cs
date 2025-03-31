using System.Collections.Generic;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    public class QuestTransition : ScriptableObject
    {
        [Header("Required Flags")]
        public List<FlagInfo> requiredFlags;

        [Header("Next Quest")] 
        public QuestInfo nextQuest;
    }
}