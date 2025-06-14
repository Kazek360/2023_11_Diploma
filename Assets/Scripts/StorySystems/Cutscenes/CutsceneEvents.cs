using System;

namespace Cutscenes
{
    public static class CutsceneEvents
    {
        public static event Action<CutsceneSO> OnCutsceneStarted;
        public static void StartCutscene(CutsceneSO cutscene)
        {
            OnCutsceneStarted?.Invoke(cutscene);
        }
        public static event Action<CutsceneSO> OnCutsceneFinished;
        public static void FinishCutscene(CutsceneSO cutscene)
        {
            OnCutsceneFinished?.Invoke(cutscene);
        }
        
        public static event Action OnCutsceneSkipped;
        public static void SkipCutscene()
        {
            OnCutsceneSkipped?.Invoke();
        }
    }
}
