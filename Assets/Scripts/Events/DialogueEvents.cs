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
}
