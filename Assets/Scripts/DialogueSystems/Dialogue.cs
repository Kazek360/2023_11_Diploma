using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public DialogueSO Info;
    public DialogueState State;
    
    public Dialogue(DialogueSO info)
    {
        Info = info;
        if (Info.requirements.Count == 0)
        {
            State = DialogueState.CanTrigger;
        }
        else
        {
            State = DialogueState.CannotTrigger;
        }
    }
}
