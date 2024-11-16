using System;

namespace DialogueSystem.Events
{
    public static class DialogueEvents
    {
        public static event Action<string> OnDialogueTriggered;
        public static void TriggerDialogue(string triggerName)
        {
            OnDialogueTriggered?.Invoke(triggerName);
        }
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
    }

}
