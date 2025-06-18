using InteractionSystem;
using UnityEngine;

namespace Quests.QuestSteps.InteractionSteps
{
    public class InteractionWithEnableStep : InteractionStep
    {
        private GameObject _objectToEnable;
        
        private void Start()
        {
            _objectToEnable = GameObject.Find(objectToInteractName);
            var trigger  = _objectToEnable.GetComponent<InteractionObject>();
            trigger.EnableInteraction();
        }
    }
}