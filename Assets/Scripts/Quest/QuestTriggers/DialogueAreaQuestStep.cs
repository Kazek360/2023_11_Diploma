using UnityEngine;

public class DialogueAreaQuestStep : DialogueQuestStep
{
    [SerializeField] private string areaName;
    private GameObject _dialogueArea;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        _dialogueArea = GameObject.Find(areaName);
        requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    private void FinishStep(DialogueSO dialogue)
    {
        if (requiredDialogue.id == dialogue.id)
        {
            Destroy(_dialogueArea);
            Finish();
        } 
    }
}
