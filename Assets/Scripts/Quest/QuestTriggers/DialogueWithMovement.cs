using System.Collections.Generic;
using UnityEngine;

public class DialogueWithMovement : QuestStep
{
    [SerializeField] private List<DialogueSO> dialoguesInfo;
    private DialogueSO _requiredDialogue;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        _requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(_requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }

    private DialogueSO ChooseRequiredDialogue()
    {
        foreach (var dialogue in dialoguesInfo)
        {
            bool allFlagsTriggered = true;
            foreach (var dialogueFlag in dialogue.requiredFlags)
            {
                if (!DialogueMarkerManager.GetMarkerById(dialogueFlag.id)._isTriggered)
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
        if (_requiredDialogue.id == dialogue.id)
        {
            PathEvents.StartMovement();
            Finish();
        } 
    }
}