using InteractionSystem.Unique;
using Quests.QuestSteps.InteractionSteps;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class MillGearAssembleStep : InteractionStep
    {
        private GameObject _objectToEnable;
        
        private void Start()
        {
            _objectToEnable = GameObject.Find(objectToInteractName);
            var interaction  = _objectToEnable.GetComponent<MillGear>();
            interaction.EnableInteraction();
        }
        
        protected override void FinishStep(string interactionName)
        {
            if (objectToInteractName == interactionName)
            {
                MillGear.InvokeOnGearFixed();
                FinishAndDelete();
            } 
        }
    }
}