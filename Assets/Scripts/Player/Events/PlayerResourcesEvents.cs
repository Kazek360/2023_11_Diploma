using System;

public class PlayerResourcesEvents
{
    public static event Action<int> OnSkillPointsValueChanged;
    public static void InvokeOnSkillPointsValueChanged(int value)
    {
        OnSkillPointsValueChanged?.Invoke(value);
    }
}
