using UnityEngine;
using StorySystems.Dialogues;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueStep : BaseQuestStep
    {
        [SerializeField] protected string triggerName;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            DialogueEvent.OnQuestDialogueFinished += FinishStep;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueEvent.OnQuestDialogueFinished -= FinishStep;
        }

        protected virtual void FinishStep(string finishedDialogueTrigger)
        {
            if (triggerName.Equals(finishedDialogueTrigger))
            {
                FinishAndDelete();
            } 
        }
    }
}