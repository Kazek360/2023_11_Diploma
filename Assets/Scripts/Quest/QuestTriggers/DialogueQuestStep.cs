using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestStep : QuestStep
{
    [SerializeField] private List<DialogueSO> dialoguesInfo;
    private DialogueSO _requiredDialogue;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        foreach (var dialogue in dialoguesInfo)
        {
            if (dialogue.requiredFlag is not null)
            {
                if (DialogueMarkerManager.GetMarkerById(dialogue.requiredFlag.id)._isTriggered)
                {
                    _requiredDialogue = dialogue;
                    break;
                }
            }
        }
        
        if (_requiredDialogue is null)
        {
            _requiredDialogue = dialoguesInfo[0];
        }
        
        DialogueEvents.UnlockDialogue(_requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }

    private void FinishStep(DialogueSO dialogue)
    {
        if (_requiredDialogue.id == dialogue.id)
        {
            Finish();
        } 
    }
}
