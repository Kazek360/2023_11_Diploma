using UnityEngine;
using DialogueSystem.Events;

public class DialogueQuestStepCliffDisabler : DialogueQuestStep
{
    private GameObject _cliffArea;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        requiredDialogue = ChooseRequiredDialogue();

        _cliffArea = GameObject.Find("CliffArea");
        _cliffArea.SetActive(false);
        
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
            _cliffArea.SetActive(true);
            Finish();
        } 
    }
}
