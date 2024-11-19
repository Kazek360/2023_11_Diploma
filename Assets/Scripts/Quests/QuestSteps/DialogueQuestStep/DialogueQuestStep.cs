using UnityEngine;
using Dialogue;

public class DialogueQuestStep : BaseQuestStep
{
    [SerializeField] protected DialogueInfo dialoguesInfo;
    protected override void OnEnable()
    {
        base.OnEnable();
        DialogueEvents.OnDialogueFinished += FinishStep;
        DialogueEvents.InvokeOnDialogueUnlocked(dialoguesInfo.triggerName, dialoguesInfo.name);
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
