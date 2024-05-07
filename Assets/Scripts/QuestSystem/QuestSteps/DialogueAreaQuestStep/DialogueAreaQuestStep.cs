using UnityEngine;
using DialogueSystem.Events;

public class DialogueAreaQuestStep : DialogueQuestStep
{
    [SerializeField] protected string areaToEnterName;
    protected GameObject area;
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += AreaEntered;
        DialogueEvents.OnDialogueFinished += FinishStep;
        area = GameObject.Find(areaToEnterName);
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= AreaEntered;
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }

    protected void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName))
        {
            AreaEvent.EnterCorrectArea(areaToEnterName);
        }
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Destroy(area);
            Finish();
        } 
    }
}
