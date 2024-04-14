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
        
        for (var i = 0; i < areasToDisableNames.Count; i++)
        {
            areasToDisable.Add(GameObject.Find(areasToDisableNames[i]));
            areasToDisable[i].SetActive(false);
        }
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
            if (((Ink.Runtime.BoolValue) DialogueUI.instance.GetVariableState(delayFlag)).value)
            {
                yield return new WaitForSeconds(1f);
            }
            for (var i = 0; i < areasToDisableNames.Count; i++)
            {
                areasToDisable[i].SetActive(true);
            }
            PathEvents.StartMovement();
            Finish();
        } 
    }
}