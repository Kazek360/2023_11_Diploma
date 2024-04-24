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
        public static event Action OnCutsceneFinished;
        public static void FinishCutscene()
        {
            OnCutsceneFinished?.Invoke();
        }
    }
 
}
