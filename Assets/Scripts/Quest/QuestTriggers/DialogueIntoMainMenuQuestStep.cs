using DialogueSystem.Events;
using UnityEngine.SceneManagement;

public class DialogueIntoMainMenuQuestStep : DialogueQuestStep
{
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
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
            SceneManager.LoadScene(0);
            Finish();
        } 
    }
}
