using UnityEngine;

public class DialogueQuestStep : QuestStep
{
    [SerializeField] private DialogueSO dialogueInfo;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueStarted += FinishStep;
        DialogueEvents.UnlockDialogue(dialogueInfo.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueStarted -= FinishStep;
    }

    private void FinishStep(Dialogue dialogue)
    {
        if (dialogueInfo.id == dialogue.Info.id)
        {
            Finish();
        } 
    }
}
