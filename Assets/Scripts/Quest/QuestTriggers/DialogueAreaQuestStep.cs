using UnityEngine;
using DialogueSystem.Events;

public class DialogueAreaQuestStep : DialogueQuestStep
{
    [SerializeField] private string areaName;
    private GameObject _dialogueArea;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        _dialogueArea = GameObject.Find(areaName);
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
            Destroy(_dialogueArea);
            Finish();
        } 
    }
}
