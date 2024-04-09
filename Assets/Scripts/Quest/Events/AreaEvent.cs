using System;

public static class AreaEvent
{
    public static event Action<string> OnAreaEnter;
    public static void EnterArea(string objectName)
    {
        OnAreaEnter?.Invoke(objectName);
    }
}
