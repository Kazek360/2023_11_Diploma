using System;

namespace Quests
{
    public static class QuestEvents
    {
        public static event Action<string> OnQuestStart;
        public static void StartQuest(string id)
        {
            OnQuestStart?.Invoke(id);
        }
        public static event Action<Quest> OnQuestStarted;
        public static void StartQuest(Quest quest)
        {
            OnQuestStarted?.Invoke(quest);
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
        
        public static event Action<Quest> OnQuestFinished;
        public static void FinishQuest(Quest quest)
        {
            OnQuestFinished?.Invoke(quest);
        }
        
        public static event Action<Quest> OnQuestAdded;
        public static void AddQuestToLog(Quest quest)
        {
            OnQuestAdded?.Invoke(quest);
        }
        
        public static event Action OnQuestTracking;
        public static void TrackQuest()
        {
            OnQuestTracking?.Invoke();
        }
        
        public static event Action<string> OnQuestStepProgress;
        public static void ProgressQuestStep(string questID)
        {
            OnQuestStepProgress?.Invoke(questID);
        }
        public static event Action OnQuestStepsDelete;
        public static void DeleteQuestSteps()
        {
            OnQuestStepsDelete?.Invoke();
        }
    }
}
