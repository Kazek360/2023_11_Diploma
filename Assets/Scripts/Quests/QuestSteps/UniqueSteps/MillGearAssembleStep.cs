using InteractionSystem.Unique;
using Quests.QuestSteps.InteractionSteps;

namespace Quests.QuestSteps.UniqueSteps
{
    public class MillGearAssembleStep : InteractionStep
    {
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