using System.Collections.Generic;

namespace DialogueSystem
{
    public class DialogueEntity
    {
        public List<Dialogue> dialogues;
        public Dialogue currentDialogue;

        public DialogueEntity()
        {
            dialogues = new List<Dialogue>();
        }
        
        public void ChangeCurrentDialogue()
        {
            foreach (var dialogue in dialogues)
            {
                if (currentDialogue == null)
                {
                    currentDialogue = dialogue;
                    continue;
                }
                if (dialogue.state == Dialogue.State.CanTrigger)
                {
                    if (dialogue.currentPriority > currentDialogue.currentPriority ||
                        currentDialogue.state == Dialogue.State.CannotTrigger ||
                        currentDialogue.state == Dialogue.State.Finished)
                    {
                        currentDialogue = dialogue;
                    }
                }
            }
        }
    }
}