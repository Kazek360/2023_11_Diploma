using System.Collections;
using UnityEngine;
using DialogueSystem.Events;

public class DialogueQuestFinishingStep : DialogueQuestStep
{
    [SerializeField] private QuestSO questToStart;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
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
        if (dialoguesInfo.name == dialogue.name)
        {
            QuestEvents.ProgressQuest(dialogue.relatedQuest.id);
            yield return new WaitForSeconds(3f);
            QuestEvents.StartQuest(questToStart.id);
            Destroy(gameObject);
        }
    }
}
