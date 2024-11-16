using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem;

public class DialogueQuestStep : BaseQuestStep
{
    [SerializeField] protected DialogueInfo dialoguesInfo;
    protected override void OnEnable()
    {
        base.OnEnable();
        DialogueEvents.OnDialogueFinished += FinishStep;
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        DialogueEvents.OnDialogueFinished -= FinishStep;
    }

    protected virtual void FinishStep(DialogueInfo dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            FinishAndDelete();
        } 
    }
}
