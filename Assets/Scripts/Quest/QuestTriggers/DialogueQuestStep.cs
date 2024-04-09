using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestStep : QuestStep
{
    [SerializeField] protected List<DialogueSO> dialoguesInfo;
    protected DialogueSO requiredDialogue;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }

    protected DialogueSO ChooseRequiredDialogue()
    {
        foreach (var dialogue in dialoguesInfo)
        {
            bool allFlagsTriggered = true;
            foreach (var dialogueFlag in dialogue.requiredFlags)
            {
                if (!FlagManager.GetMarkerById(dialogueFlag.id)._isTriggered)
                { 
                    allFlagsTriggered = false;
                    break;
                }
            }
            if (allFlagsTriggered)
            {
                return dialogue;
            }
        }
        return dialoguesInfo[0];
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (requiredDialogue.id == dialogue.id)
        {
            Finish();
        } 
    }
}
