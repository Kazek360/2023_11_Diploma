
using UnityEngine.SceneManagement;

public class DialogueIntoMainMenuQuestStep : DialogueQuestStep
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
            SceneManager.LoadScene(0);
            Finish();
        } 
    }
}
