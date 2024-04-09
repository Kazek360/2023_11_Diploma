using System;

public static class CutsceneEvents
{
    public static event Action<CutsceneSO> OnCutsceneOpen;
    public static void OpenCutscene(CutsceneSO cutscene)
    {
        OnCutsceneOpen?.Invoke(cutscene);
    }
    public static event Action OnCutsceneClose;
    public static void CloseCutscene()
    {
        OnCutsceneClose?.Invoke();
    }
}
