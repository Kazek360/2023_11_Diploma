using System.Collections;
using System.Collections.Generic;
using Dialogues;
using UnityEngine;

namespace Quests.QuestSteps.DialogueSteps
{
    public class DialoguesStep : BaseQuestStep
    {
        [SerializeField] protected List<DialogueInfo> dialogues;
        protected int currentDialogueIndex;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            currentDialogueIndex = 0;
            DialogueEvent.OnDialogueFinished += CheckIfCorrectDialogue;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueEvent.OnDialogueFinished -= CheckIfCorrectDialogue;
        }
        private IEnumerator Start()
        {
            while (!DialogueManager.isInitialized)
            {
                yield return null;
            }
            DialogueEvent.UnlockDialogue(dialogues[currentDialogueIndex].triggerName, 
                dialogues[currentDialogueIndex].name);
        }
        
        private void CheckIfCorrectDialogue(DialogueInfo dialogue)
        {
            if (dialogues[currentDialogueIndex].name == dialogue.name)
            {
                NextDialogue();
            } 
        }

        protected virtual void NextDialogue()
        {
            currentDialogueIndex++;
            if (currentDialogueIndex == dialogues.Count)
            {
                FinishAndDelete();
            }
            else
            {
                DialogueEvent.UnlockDialogue(dialogues[currentDialogueIndex].triggerName, 
                    dialogues[currentDialogueIndex].name);
            }
        }
    }
}