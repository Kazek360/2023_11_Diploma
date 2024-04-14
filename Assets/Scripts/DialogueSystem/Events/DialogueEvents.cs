using System;

namespace DialogueSystem.Events
{
    public static class DialogueEvents
    {
        public static event Action<DialogueSO> OnDialogueStarted;
        public static void StartDialogue(DialogueSO dialogue)
        {
            OnDialogueStarted?.Invoke(dialogue);
        }
        public static event Action<DialogueSO> OnDialogueFinished;
        public static void FinishDialogue(DialogueSO dialogue)
        {
            OnDialogueFinished?.Invoke(dialogue);
        }
        public static event Action<string> OnDialogueUnlocked;
        public static void UnlockDialogue(string id)
        {
            OnDialogueUnlocked?.Invoke(id);
        }
    }

}
