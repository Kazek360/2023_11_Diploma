using System;

public class PlayerResourcesEvents
{
    public static event Action<int> OnSkillPointsChange;
    public static void ChangeSkillPointsValue(int value)
    {
        OnSkillPointsChange?.Invoke(value);
    }
}
