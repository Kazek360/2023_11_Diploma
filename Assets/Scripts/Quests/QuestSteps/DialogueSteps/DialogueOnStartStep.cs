using System.Collections;
using Dialogues;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueOnStartStep : DialogueStep
    {
        protected override IEnumerator Start()
        {
            yield return base.Start();
            
            DialogueManager.Instance.TriggerDialogue(dialogueInfo.triggerName);
        }
    }
}