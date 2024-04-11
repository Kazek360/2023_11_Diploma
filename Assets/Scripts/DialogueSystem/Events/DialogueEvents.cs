using System;

namespace DialogueSystem.Events
{
    public static class DialogueEvents
    {
        public static event Action<Dialogue> OnDialogueStarted;
        public static void StartDialogue(Dialogue dialogue)
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
    
        public static event Action<string> OnDialogueMarkerTrigger;
        public static void TriggerDialogueMarker(string id)
        {
            OnDialogueMarkerTrigger?.Invoke(id);
        }
        public static event Action<string> OnDialogueMarkerStateChange;
        public static void ChangeDialogueMarkerState(string id)
        {
            OnDialogueMarkerStateChange?.Invoke(id);
        }
    
        public static event Action OnRaceBegin;
        public static void BeginRace()
        {
            OnRaceBegin?.Invoke();
        }
    }

}
