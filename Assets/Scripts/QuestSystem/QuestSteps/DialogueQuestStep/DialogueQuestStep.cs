using System.Collections.Generic;
using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;

public class DialogueQuestStep : BaseQuestStep
{
    [SerializeField] protected DialogueSO dialoguesInfo;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Finish();
        } 
    }
}
