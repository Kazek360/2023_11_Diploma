
using System.Collections;
using UnityEngine;

public class DialogueWithDelayedMovement : DialogueQuestStep
{
    [SerializeField] private FlagSO flagDelay;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= StartFinishingStep;
    }
    
    private void StartFinishingStep(DialogueSO dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(DialogueSO dialogue)
    {
        if (requiredDialogue.id == dialogue.id)
        {
            if (FlagManager.GetMarkerById(flagDelay.id)._isTriggered)
            {
                yield return new WaitForSeconds(1f);
            }
            PathEvents.StartMovement();
            Finish();
        } 
    }
}