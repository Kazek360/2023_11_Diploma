
public class DialogueWithMovement : DialogueQuestStep
{
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
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
            PathEvents.StartMovement();
            Finish();
        } 
    }
}