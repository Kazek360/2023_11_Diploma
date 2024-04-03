using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestEndStep : QuestStep
{
    [SerializeField] private DialogueSO dialogueInfo;
    [SerializeField] private QuestSO questToStart;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueStarted += StartFinishingStep;
        DialogueEvents.UnlockDialogue(dialogueInfo.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueStarted -= StartFinishingStep;
    }
    
    private void StartFinishingStep(Dialogue dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(Dialogue dialogue)
    {
        if (dialogueInfo.id == dialogue.Info.id)
        {
            QuestEvents.ProgressQuest(dialogue.Info.relatedQuest.id);
            yield return new WaitForSeconds(3f);
            QuestEvents.StartQuest(questToStart.id);
            Destroy(gameObject);
        }
    }
}
