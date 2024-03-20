using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueEventsNew
{
    public static event Action<DialogueNew> OnDialogueStarted;
    public static void StartDialogue(DialogueNew dialogue)
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
