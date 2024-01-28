using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQuestStep : QuestStep
{
    [SerializeField] private string characterId;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishDialogue;
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished -= FinishDialogue;
    }

    private void FinishDialogue(string id)
    {
        Finish();
    }
}
