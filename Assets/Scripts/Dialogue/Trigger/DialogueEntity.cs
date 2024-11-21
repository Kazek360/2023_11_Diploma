using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Trigger
{
    public class DialogueEntity : MonoBehaviour
    {
        [Header("Name")]
        public string triggerName;
        
        [NonSerialized] public List<Dialogue> dialogues;
        [NonSerialized] public Dialogue currentDialogue;

        protected void Awake()
        {
            dialogues = new List<Dialogue>();
        }
        
        public virtual void ChangeCurrentDialogue()
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
        
        protected void StartDialogue()
        {
            DialogueEvents.StartDialogue(currentDialogue.info);
            
            if (currentDialogue.info.dialogueType != DialogueInfo.Type.Repeatable)
            {
                currentDialogue.state = Dialogue.State.Finished;
                ChangeCurrentDialogue();
            }
        }
        
    }
}