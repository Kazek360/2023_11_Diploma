using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueEvents
{
    public static event Action<Dialogue> OnDialogueStart;
    
    public static void StartDialogue(Dialogue dialogue)
    {
        OnDialogueStart?.Invoke(dialogue);
    }
    
    public static event Action<string> OnDialogueFinished;

    public static void DialogueFinished(string id)
    {
        OnDialogueFinished?.Invoke(id);
    }
    
    public static event Action<Dialogue> OnDialogueStateChange;
    public static void ChangeDialogueState(Dialogue dialogue)
    {
        OnDialogueStateChange?.Invoke(dialogue);
    }
}
