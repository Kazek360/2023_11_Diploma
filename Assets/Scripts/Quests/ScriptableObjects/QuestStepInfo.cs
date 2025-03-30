using System.Collections.Generic;
using UnityEngine;

namespace Quests.ScriptableObjects
{
    public class QuestStepInfo : ScriptableObject
    {
        [Header("General")]
        [TextArea] public string description;

        [Header("Repeat")]
        public int repeatAmount;
        
        [Header("Prefab")] 
        public GameObject prefab;

        [Header("Transitions")] 
        public List<QuestStepTransition> transitions;
    }
}
