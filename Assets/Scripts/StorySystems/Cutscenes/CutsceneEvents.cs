using System;

namespace StorySystems.Cutscenes
{
    public static class CutsceneEvents
    {
        public static event Action<Cutscene> OnCutsceneStarted;
        public static void StartCutscene(Cutscene cutscene)
        {
            OnCutsceneStarted?.Invoke(cutscene);
        }
        public static event Action<Cutscene> OnCutsceneFinished;
        public static void FinishCutscene(Cutscene cutscene)
        {
            OnCutsceneFinished?.Invoke(cutscene);
        }
        
        public static event Action OnCutsceneSkipped;
        public static void SkipCutscene()
        {
            OnCutsceneSkipped?.Invoke();
        }
        
        public static event Action OnDialogueClipStarted;
        public static void InvokeOnDialogueClipStarted()
        {
            OnDialogueClipStarted?.Invoke();
        }
        public static event Action OnDialogueClipFinished;
        public static void InvokeOnDialogueClipFinished()
        {
            OnDialogueClipFinished?.Invoke();
        }
    }
}
