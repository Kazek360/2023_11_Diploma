using System.Collections.Generic;
using UnityEngine;

public class DialogueAreaQuestStep : QuestStep
{
    [SerializeField] private List<DialogueSO> dialoguesInfo;
    [SerializeField] private string areaName;
    private DialogueSO _requiredDialogue;
    private GameObject _dialogueArea;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        _dialogueArea = GameObject.Find(areaName);
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
            Destroy(_dialogueArea);
            Finish();
        } 
    }
}
