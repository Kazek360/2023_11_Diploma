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
        foreach (var dialogue in dialoguesInfo)
        {
            if (dialogue.requiredFlag is not null)
            {
                if (DialogueMarkerManager.GetMarkerById(dialogue.requiredFlag.id)._isTriggered)
                {
                    _requiredDialogue = dialogue;
                    break;
                }
            }
        }

        if (_requiredDialogue is null)
        {
            _requiredDialogue = dialoguesInfo[0];
        }
        
        DialogueEvents.UnlockDialogue(_requiredDialogue.id);
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
        if (_requiredDialogue.id == dialogue.id)
        {
            QuestEvents.ProgressQuest(dialogue.relatedQuest.id);
            yield return new WaitForSeconds(3f);
            QuestEvents.StartQuest(questToStart.id);
            Destroy(gameObject);
        }
    }
}
