using InteractionSystem;
using UnityEngine;

namespace Quests.QuestSteps.InteractionSteps
{
    public class InteractionStep : BaseQuestStep
    {
        [SerializeField] protected string objectToInteractName;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            InteractionEvents.OnInteractionEntered += FinishStep;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            InteractionEvents.OnInteractionEntered -= FinishStep;
        }
        
        protected virtual void FinishStep(string interactionName)
        {
            if (objectToInteractName == interactionName)
            {
                FinishAndDelete();
            } 
        }
    }
}