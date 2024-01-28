using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueEvents
{
    public static event Action<string> OnDialogueFinished;

    public static void DialogueFinished(string id)
    {
        OnDialogueFinished?.Invoke(id);
    }
}
