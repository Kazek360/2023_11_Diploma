using UnityEngine;
using Dialogues;

public class AreaDialogueStep : DialogueStep
{
    [SerializeField] protected string areaToEnterName;
    protected override void OnEnable()
    {
        base.OnEnable();
        AreaEvent.OnAreaEnter += AreaEntered;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AreaEvent.OnAreaEnter -= AreaEntered;
    }
    
    protected virtual void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName))
        {
            DialogueManager.Instance.TriggerDialogue(dialoguesInfo.triggerName);
        }
    }
    
    protected override void FinishStep(DialogueInfo dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            FinishAndDelete();
        } 
    }
}
