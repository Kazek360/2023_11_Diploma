using System.Collections;
using UnityEngine;
using StorySystems.Dialogues;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialogueStep : BaseQuestStep
    {
        [SerializeField] protected DialogueInfo dialogueInfo;
    
        protected override void OnEnable()
        {
            base.OnEnable();
            DialogueEvent.OnDialogueFinished += FinishStep;
        }

        protected virtual IEnumerator Start()
        {
            while (!DialogueManager.isInitialized)
            {
                yield return null;
            }
            DialogueEvent.UnlockDialogue(dialogueInfo.triggerName, dialogueInfo.name);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueEvent.OnDialogueFinished -= FinishStep;
        }

        protected virtual void FinishStep(DialogueInfo dialogue)
        {
            if (dialogueInfo.name == dialogue.name)
            {
                FinishAndDelete();
            } 
        }
    }
}