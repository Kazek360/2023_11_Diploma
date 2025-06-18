using InteractionSystem.Unique;
using Quests.QuestSteps.InteractionSteps;
using UnityEngine;

namespace Quests.QuestSteps.UniqueSteps
{
    public class MillGearAssembleStep : InteractionStep
    {
        private GameObject _objectToEnable;
        private MillGear _millGear;
        
        private void Start()
        {
            _objectToEnable = GameObject.Find(objectToInteractName);
            _millGear  = _objectToEnable.GetComponent<MillGear>();
            _millGear.EnableInteraction();
        }
        
        protected override void FinishStep(string interactionName)
        {
            if (objectToInteractName == interactionName)
            {
                _millGear.FixGear();
                FinishAndDelete();
            } 
        }
    }
}