using System.Collections;
using UnityEngine;

public class DialogueQuestFinishingStep : DialogueQuestStep
{
    [SerializeField] private QuestSO questToStart;
    
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
            QuestEvents.ProgressQuest(dialogue.relatedQuest.id);
            yield return new WaitForSeconds(3f);
            QuestEvents.StartQuest(questToStart.id);
            Destroy(gameObject);
        }
    }
}
