using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueEvents
{
    public static event Action<Dialogue> OnDialogueStarted;
    public static void StartDialogue(Dialogue dialogue)
    {
        OnDialogueStarted?.Invoke(dialogue);
    }
    
    public static event Action OnDialogueEnd;
    public static void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
    }
    
    public static event Action<string> OnDialogueUnlocked;
    public static void UnlockDialogue(string id)
    {
        OnDialogueUnlocked?.Invoke(id);
    }
}
