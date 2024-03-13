using UnityEngine;

//Class storing NPC dialogue info and state of dialogue.
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
