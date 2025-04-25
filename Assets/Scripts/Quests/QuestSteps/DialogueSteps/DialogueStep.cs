using System.Collections;
using UnityEngine;
using Dialogues;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueStep : BaseQuestStep
    {
        [SerializeField] protected DialogueInfo dialoguesInfo;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            DialogueEvent.OnDialogueFinished += FinishStep;
        }

        private IEnumerator Start()
        {
            while (!DialogueManager.isInitialized)
            {
                yield return null;
            }
            DialogueEvent.UnlockDialogue(dialoguesInfo.triggerName, dialoguesInfo.name);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueEvent.OnDialogueFinished -= FinishStep;
        }

        protected virtual void FinishStep(DialogueInfo dialogue)
        {
            if (dialoguesInfo.name == dialogue.name)
            {
                FinishAndDelete();
            } 
        }
    }
}