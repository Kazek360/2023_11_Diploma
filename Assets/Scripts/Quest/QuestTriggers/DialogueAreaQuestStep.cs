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
            Destroy(_dialogueArea);
            Finish();
        } 
    }
}
