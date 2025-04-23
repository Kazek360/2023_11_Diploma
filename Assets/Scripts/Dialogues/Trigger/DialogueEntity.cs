using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues.Trigger
{
    /// <summary>
    /// Klasa przechowująca informacje o dialogach dla określonej postaci
    /// </summary>
    public class DialogueEntity : MonoBehaviour
    {
        [Header("Name")]
        public string triggerName;
        
        [NonSerialized] public List<Dialogue> dialogues;
        [NonSerialized] public Dialogue currentDialogue;

        private void Awake()
        {
            dialogues = new List<Dialogue>();
        }

        protected virtual void OnEnable()
        {
            DialogueEvent.OnManagerPinged += SubscribeToManager;
        }
        protected virtual void OnDisable()
        {
            DialogueEvent.OnManagerPinged -= SubscribeToManager;
        }

        private void SubscribeToManager()
        {
            DialogueEvent.SubscribeToManager(triggerName, this);
        }
        
        /// <summary>
        /// Metoda zmieniająca obecny dialog
        /// </summary>
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
        
        /// <summary>
        /// Metoda rozpoczynająca obecny dialog i zmieniająca jego stan
        /// </summary>
        public void StartDialogue()
        {
            DialogueEvent.StartDialogue(currentDialogue.info);
            
            if (currentDialogue.info.dialogueType != DialogueInfo.Type.Repeatable)
            {
                currentDialogue.state = Dialogue.State.Finished;
                ChangeCurrentDialogue();
            }
        }
    }
}