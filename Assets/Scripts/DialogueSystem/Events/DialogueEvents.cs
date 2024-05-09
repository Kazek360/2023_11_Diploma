using System;
using DialogueSystem.ScriptableObjects;

namespace DialogueSystem.Events
{
    public static class DialogueEvents
    {
        public static event Action<string> OnDialogueTriggered;
        public static void TriggerDialogue(string triggerName)
        {
            OnDialogueTriggered?.Invoke(triggerName);
        }
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
