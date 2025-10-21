using StorySystems.Dialogues;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueOnStartStep : DialogueStep
    {
        private void Start()
        {
            DialogueEvent.TriggerDialogue(triggerName);
        }
    }
}