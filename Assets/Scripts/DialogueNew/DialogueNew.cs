using UnityEngine;

//Class storing NPC dialogue info and state of dialogue.
public class DialogueNew
{
    public DialogueSONew Info;
    public DialogueStateNew State;
    
    public DialogueNew(DialogueSONew info)
    {
        Info = info;
        State = DialogueStateNew.NotUnlocked;
    }
}
