using Other;
using Quests.QuestSteps.DialogueSteps;

namespace Quests.QuestSteps.UniqueSteps
{
    public class ShoreAwakening : AreaDialogueStep
    {
        protected void Start()
        {
            Nestor.ChangeNestorState("shore", true);
        }
    }
}