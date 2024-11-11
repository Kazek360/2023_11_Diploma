using System.Collections.Generic;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    public class QuestStepInfo : ScriptableObject
    {
        [Header("Required Flags")]
        public List<FlagInfo> requiredFlags;
        
        [Header("General")]
        [TextArea] public string description;

        [Header("Repeat")]
        public int repeatAmount;
        
        [Header("Prefab")] 
        public GameObject prefab;

        [Header("Next Steps")] 
        public List<QuestStepInfo> nextSteps;
    }
}
