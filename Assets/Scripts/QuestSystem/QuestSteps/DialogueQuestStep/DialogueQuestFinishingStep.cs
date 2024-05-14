using System.Collections;
using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;

public class DialogueQuestFinishingStep : DialogueQuestStep
{
    [SerializeField] private QuestSO questToStart;
    
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
            Finish();
            QuestEvents.StartQuest(questToStart.id);
            Delete();
        }
    }
}
