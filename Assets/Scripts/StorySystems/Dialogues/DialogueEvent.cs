using System;
using StorySystems.Dialogues.Trigger;

namespace StorySystems.Dialogues
{
    public static class DialogueEvent
    {
        public static event Action<DialogueInfo> OnDialogueStarted;
        public static void StartDialogue(DialogueInfo dialogue)
        {
            OnDialogueStarted?.Invoke(dialogue);
        }
        public static event Action<DialogueInfo> OnDialogueFinished;
        public static void FinishDialogue(DialogueInfo dialogue)
        {
            OnDialogueFinished?.Invoke(dialogue);
        }
        public static event Action OnDialogueContinued;
        public static void ContinueDialogue()
        {
            OnDialogueContinued?.Invoke();
        }
        public static event Action<string, string> OnDialogueUnlocked;
        public static void UnlockDialogue(string triggerName, string dialogueName)
        {
            OnDialogueUnlocked?.Invoke(triggerName, dialogueName);
        }
        
        public static event Action OnManagerPinged;
        public static void Ping()
        {
            OnManagerPinged?.Invoke();
        }
        public static event Action<string, DialogueEntity> OnEntitySubscribed;
        public static void SubscribeToManager(string triggerName, DialogueEntity entity)
        {
            OnEntitySubscribed?.Invoke(triggerName, entity);
        }
    }
}
