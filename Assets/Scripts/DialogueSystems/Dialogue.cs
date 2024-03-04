using UnityEngine;

public class Dialogue
{
    public DialogueSO Info;
    public DialogueState State;
    
    public Dialogue(DialogueSO info)
    {
        Info = info;
        State = DialogueState.NotUnlocked;
    }
}
