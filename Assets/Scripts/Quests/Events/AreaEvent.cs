using System;

public static class AreaEvent
{
    public static event Action<string, string> OnAreaEnter;
    public static void EnterArea(string areaName, string objectName)
    {
        OnAreaEnter?.Invoke(areaName, objectName);
    }
    
    public static event Action<string> OnCorrectAreaEnter;
    public static void EnterCorrectArea(string areaName)
    {
        OnCorrectAreaEnter?.Invoke(areaName);
    }
    
    public static event Action<string, string> OnDialogueAreaEnter;
    public static void EnterDialogueArea(string areaName, string objectName)
    {
        OnDialogueAreaEnter?.Invoke(areaName, objectName);
    }
    
}
