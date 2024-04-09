using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainQuestDialogueQuestEndStep : QuestStep
{
    [SerializeField] private List<DialogueSO> dialoguesInfo;
    private DialogueSO _requiredDialogue;
    [SerializeField] private QuestSO questToStart;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        _requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(_requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished -= StartFinishingStep;
    }
    
    private DialogueSO ChooseRequiredDialogue()
    {
        foreach (var dialogue in dialoguesInfo)
        {
            bool allFlagsTriggered = true;
            foreach (var dialogueFlag in dialogue.requiredFlags)
            {
                if (!DialogueMarkerManager.GetMarkerById(dialogueFlag.id)._isTriggered)
                { 
                    allFlagsTriggered = false;
                    break;
                }
            }
            if (allFlagsTriggered)
            {
                return dialogue;
            }
        }
        return dialoguesInfo[0];
    }
    
    private void StartFinishingStep(DialogueSO dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(DialogueSO dialogue)
    {
        if (_requiredDialogue.id == dialogue.id)
        {
            QuestEvents.ProgressQuest(dialogue.relatedQuest.id);
            yield return new WaitForSeconds(3f);
            QuestEvents.StartQuest(questToStart.id);
            Destroy(gameObject);
        }
    }
}
