using System;
using UnityEngine;

namespace StorySystems.Dialogues
{
    public static class DialogueEvent
    {
        //Events triggered, when dialogue starts
        public static event Action OnDialogueInitialized;
        public static void InitializeDialogue()
        {
            OnDialogueInitialized?.Invoke();
        }
        public static event Action<string, TextAsset> OnDialogueStarted;
        public static void StartDialogue(string trigger, TextAsset text)
        {
            OnDialogueStarted?.Invoke(trigger, text);
        }
        
        //Events triggered, when dialogue ends
        public static event Action OnDialogueFinished;
        public static void FinishDialogue()
        {
            OnDialogueFinished?.Invoke();
        }
        public static event Action<string> OnQuestDialogueFinished;
        public static void FinishQuestDialogue(string trigger)
        {
            OnQuestDialogueFinished?.Invoke(trigger);
        }
        
        //Other dialogue events
        public static event Action OnDialogueContinued;
        public static void ContinueDialogue()
        {
            OnDialogueContinued?.Invoke();
        }
        
        public static event Action<string, TextAsset, string> OnDialogueAreaEnter;
        public static void EnterDialogueArea(string trigger, TextAsset dialogue, string collisionName)
        {
            OnDialogueAreaEnter?.Invoke(trigger, dialogue, collisionName);
        }
        
        public static event Action<string> OnDialogueTriggered;
        public static void TriggerDialogue(string triggerName)
        {
            OnDialogueTriggered?.Invoke(triggerName);
        }
    }
}
