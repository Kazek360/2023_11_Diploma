using System.Collections.Generic;
using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;

public class DialogueQuestStep : BaseQuestStep
{
    [SerializeField] protected DialogueSO dialoguesInfo;
    protected override void OnEnable()
    {
        base.OnEnable();
        DialogueEvents.OnDialogueFinished += FinishStep;
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    protected virtual void OnDisable()
    {
        DialogueEvents.OnDialogueFinished -= FinishStep;
    }
    
    protected virtual void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            FinishAndDelete();
        } 
    }
}
