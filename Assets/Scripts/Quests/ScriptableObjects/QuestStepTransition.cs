using System.Collections.Generic;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    public class QuestStepTransition : ScriptableObject
    {
        [Header("Required Flags")]
        public List<FlagInfo> requiredFlags;

        [Header("Next Step")] 
        public QuestStepInfo nextStep;
    }
}
