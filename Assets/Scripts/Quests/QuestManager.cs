using System.Collections.Generic;
using QuestSystem;
using SaveSystem;
using SaveSystem.Data;
using Shared;
using UnityEngine;

namespace Quests
{
    public class QuestManager : Singleton<QuestManager>, IDataPersistence
    {
        private Dictionary<string, Quest> _quests;
        public Dictionary<string, Quest> Quests => _quests;

        private Quest _trackedQuest;
        public Quest TrackedQuest
        {
            get => _trackedQuest;
            set
            {
                _trackedQuest = value;
                QuestEvents.TrackQuest();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (Instance.Quests is null)
            {
                _quests = CreateQuestDictionary();
            }
        }

        private void OnEnable()
        {
            QuestEvents.OnQuestStart += StartQuest;
            QuestEvents.OnQuestProgress += ProgressQuest;
            QuestEvents.OnQuestFinish += FinishQuest;
        }

        private void OnDisable()
        {
            QuestEvents.OnQuestStart -= StartQuest;
            QuestEvents.OnQuestProgress -= ProgressQuest;
            QuestEvents.OnQuestFinish -= FinishQuest;
        }

        private bool CheckRequirementsMet(Quest quest)
        {
            var meetsRequirements = true;
            foreach (var questRequirement in quest.Info.requirements)
            {
                if (GetQuestById(questRequirement.id).State != QuestState.Finished)
                {
                    meetsRequirements = false;
                }
            }

            return meetsRequirements;
        }

        private void ChangeQuestState(string id, QuestState state)
        {
            var quest = GetQuestById(id);
            quest.State = state;
            QuestEvents.ChangeQuestState(quest);
        }

        private void StartQuest(string id)
        {
            var quest = GetQuestById(id);
            quest.InstantiateCurrentQuestStep(transform);
            ChangeQuestState(id, QuestState.InProgress);
            TrackedQuest = quest;
        }

        private void ProgressQuest(string id)
        {
            var quest = GetQuestById(id);
            quest.MoveToNextStep();
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(transform);
                if (!quest.NextStepExists())
                {
                    ChangeQuestState(id, QuestState.CanFinish);
                }
            }

            if (GetQuestById(id).Equals(TrackedQuest))
            {
                QuestEvents.TrackQuest();
            }
        }

        private void FinishQuest(string id)
        {
            PlayerStats.GetInstance().AddXp(_quests[id].Info.experience);
            foreach (var item in _quests[id].Info.items)
            {
                GameConfig.PlayerInventory.AddItem(item, 1);
            }

            ChangeQuestState(id, QuestState.Finished);
            
            foreach (var quest in _quests.Values)
            {
                if (quest.State == QuestState.RequirementsNotMet && CheckRequirementsMet(quest))
                {
                    if (quest.Info.questCategory == QuestCategory.MainQuest)
                    {
                        QuestEvents.StartQuest(quest.Info.id);
                    }
                    else
                    {
                        ChangeQuestState(quest.Info.id, QuestState.CanStart);
                    }
                }
            }
        }

        private Dictionary<string, Quest> CreateQuestDictionary()
        {
            QuestSO[] allQuests = Resources.LoadAll<QuestSO>("Quests");
            Dictionary<string, Quest> idToDictionary = new Dictionary<string, Quest>();
            foreach (var questInfo in allQuests)
            {
                if (idToDictionary.ContainsKey(questInfo.id))
                {
                    Debug.LogWarning($"Found Duplicate ID: {questInfo.id}");
                }

                idToDictionary.Add(questInfo.id, new Quest(questInfo));
            }

            return idToDictionary;
        }

        public Quest GetQuestById(string id)
        {
            var quest = _quests[id];
            if (quest is null)
            {
                Debug.LogError($"There is no quest with ID: {id}");
            }

            return quest;
        }

        public void InitializeOnNewGame()
        {
            QuestEvents.StartQuest("prologue_1");
        }

        public void LoadDataFromSaveFile(GameData gameData)
        {
            QuestEvents.DeleteQuestSteps();
            var loadedQuests = gameData.Quests;
            if (loadedQuests is not null)
            {
                foreach (var pair in loadedQuests)
                {
                    loadedQuests[pair.Key].Info = Instance.Quests[pair.Key].Info;
                }
                _quests = loadedQuests;
            }
            
            foreach (var quest in Instance.Quests.Values)
            {
                if (quest.State == QuestState.InProgress || quest.State == QuestState.CanFinish)
                {
                    QuestEvents.ChangeQuestState(quest);
                    quest.InstantiateCurrentQuestStep(transform);
                }
            }
            
            if (!gameData.trackedQuest.Equals(""))
            {
                TrackedQuest = GetQuestById(gameData.trackedQuest);
            }
            else
            {
                TrackedQuest = null;
            }
        }

        public void LoadDataBetweenScenes(GameData gameData)
        {
            foreach (var quest in Instance.Quests.Values)
            {
                QuestEvents.ChangeQuestState(quest);
            }
            QuestEvents.TrackQuest();
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.Quests = _quests;
            gameData.trackedQuest = TrackedQuest.Info.id;
        }
    }
}


