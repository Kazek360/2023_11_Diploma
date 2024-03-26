using System;

public static class QuestEvents
{
    public static event Action<string> OnQuestStart;
    public static void StartQuest(string id)
    {
        OnQuestStart?.Invoke(id);
    }

    public static event Action<string> OnQuestProgress;
    public static void ProgressQuest(string id)
    {
        OnQuestProgress?.Invoke(id);
    }
    
    public static event Action<string> OnQuestFinish;
    public static void FinishQuest(string id)
    {
        OnQuestFinish?.Invoke(id);
    }
    
    public static event Action<Quest> OnQuestStateChange;
    public static void ChangeQuestState(Quest quest)
    {
        OnQuestStateChange?.Invoke(quest);
    }

    public static event Action<string> OnQuestStepInstantation;
    
    public static void SendQuestStepData(string description)
    {
        OnQuestStepInstantation?.Invoke(description);
    }
}
