using DialogueSystem.Events;
using System.Collections;
using DialogueSystem.UI;
using UnityEngine;

public class DialogueWithDelayedMovement : DialogueQuestStep
{
    [SerializeField] private string delayFlag;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished -= StartFinishingStep;
    }
    
    private void StartFinishingStep(DialogueSO dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            if (((Ink.Runtime.BoolValue) GameConfig.DialogueUI.GetVariableState(delayFlag)).value)
            {
                yield return new WaitForSeconds(1f);
            }
            PathEvents.StartMovement();
            Finish();
        } 
    }
}