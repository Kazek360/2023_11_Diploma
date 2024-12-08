
using System.Collections.Generic;
using Dialogue;
using UnityEngine;

namespace Quests.QuestSteps.DialogueQuestStep
{
    public class DialoguesQuestStep : BaseQuestStep
    {
        [SerializeField] protected List<DialogueInfo> dialogues;
        protected int currentDialogueIndex;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            currentDialogueIndex = 0;
            DialogueEvents.OnDialogueFinished += CheckIfCorrectDialogue;
            DialogueEvents.InvokeOnDialogueUnlocked(dialogues[currentDialogueIndex].triggerName, 
                dialogues[currentDialogueIndex].name);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueEvents.OnDialogueFinished -= CheckIfCorrectDialogue;
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
                DialogueEvents.InvokeOnDialogueUnlocked(dialogues[currentDialogueIndex].triggerName, 
                    dialogues[currentDialogueIndex].name);
            }
        }
    }
}