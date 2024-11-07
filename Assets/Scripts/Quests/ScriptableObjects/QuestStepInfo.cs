using System.Collections.Generic;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    public class QuestStepInfo : ScriptableObject
    {
        [Header("General")]
        [TextArea] public string description;

        [Header("Repeatable")] 
        public bool isRepeatable;
        public int repeatAmount;
        
        [Header("Object with script")] 
        public GameObject gameObject;

        [Header("Next Steps")] 
        public List<QuestStepInfo> nextSteps;
    }
}
