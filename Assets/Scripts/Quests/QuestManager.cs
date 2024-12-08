using System.Collections.Generic;
using Quests.Flags;
using Quests.ScriptableObjects;
using SaveSystem;
using SaveSystem.Data;
using Shared;
using UnityEngine;

namespace Quests
{
    public class QuestManager : Singleton<QuestManager>, IDataPersistence
    {
        [Header("Load Globals JSON")] 
        [SerializeField] private TextAsset loadGlobals;
        public TextAsset LoadGlobals => loadGlobals;

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

        private FlagManager _flagManager;
        public FlagManager FlagManager => _flagManager;
        
        protected override void Awake()
        {
            base.Awake();
            if (Instance.Quests is null)
            {
                _quests = CreateQuestDictionary();
                _flagManager = new FlagManager(loadGlobals);
            }
        }

        private void OnEnable()
        {
            QuestEvents.OnQuestStart += StartQuest;
            QuestEvents.OnQuestProgress += ProgressQuest;
            QuestEvents.OnQuestStepProgress += UpdateQuestStepProgress;
        }

        private void OnDisable()
        {
            QuestEvents.OnQuestStart -= StartQuest;
            QuestEvents.OnQuestProgress -= ProgressQuest;
            QuestEvents.OnQuestStepProgress -= UpdateQuestStepProgress;
        }

        public bool CheckForRequiredFlagsState(List<FlagInfo> flags)
        {
            var allFlagsTriggered = true;
            foreach (var flag in flags)
            {
                if (!FlagManager.IsFlagTriggered(flag.name))
                {
                    allFlagsTriggered = false;
                }
            }

            return allFlagsTriggered;
        }

        private void ChangeQuestState(string id, QuestState state)
        {
            var quest = GetQuestById(id);
            quest.state = state;
            QuestEvents.ChangeQuestState(quest);
        }

        private void StartQuest(string id)
        {
            var quest = GetQuestById(id);
            quest.InstantiateFirstStep(transform);
            ChangeQuestState(id, QuestState.InProgress);
            TrackedQuest = quest;
        }
        
        private void ProgressQuest(string id)
        {
            var quest = GetQuestById(id);
            
            if (quest.NextStepExists())
            {
                quest.InstantiateNextStep(transform);
            }
            else
            {
                FinishQuest(id);
            }

            if (GetQuestById(id).Equals(TrackedQuest))
            {
                QuestEvents.TrackQuest();
            }
        }

        private void FinishQuest(string id)
        {
            QuestEvents.FinishQuest(id);
            var quest = GetQuestById(id);
            
            PlayerStats.GetInstance().AddXp(quest.info.experience);
            foreach (var item in quest.info.items)
            {
                GameConfig.PlayerInventory.AddItem(item, 1);
            }

            ChangeQuestState(id, QuestState.Finished);

            foreach (var nextQuestInfo in quest.info.nextQuests)
            {
                if (CheckForRequiredFlagsState(nextQuestInfo.requiredFlags))
                {
                    if (nextQuestInfo.questCategory == QuestCategory.MainQuest)
                    {
                        QuestEvents.StartQuest(nextQuestInfo.name);
                    }
                    else
                    {
                        ChangeQuestState(nextQuestInfo.name, QuestState.CanStart);
                    }
                }
            }
        }

        private void UpdateQuestStepProgress(string id)
        {
            var quest = GetQuestById(id);
            quest.UpdateStepProgress();
            if (GetQuestById(id).Equals(TrackedQuest))
            {
                QuestEvents.TrackQuest();
            }
        }
        
        private Dictionary<string, Quest> CreateQuestDictionary()
        {
            QuestInfo[] allQuests = Resources.LoadAll<QuestInfo>("Quests");
            Dictionary<string, Quest> idToDictionary = new Dictionary<string, Quest>();
            foreach (var questInfo in allQuests)
            {
                if (idToDictionary.ContainsKey(questInfo.name))
                {
                    Debug.LogWarning($"Found Duplicate ID: {questInfo.name}");
                }

                idToDictionary.Add(questInfo.name, new Quest(questInfo));
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

        public void LoadData(GameData gameData)
        {
            switch (DataPersistenceManager.Instance.CurrentLoadMode)
            {
                case LoadMode.NewGame:
                    InitializeOnNewGame();
                    break;
                case LoadMode.FromSaveFile:
                    LoadDataFromSaveFile(gameData);
                    break;
                case LoadMode.BetweenScenes:
                    LoadDataBetweenScenes(gameData);
                    break;
            }
        }
        
        private void InitializeOnNewGame()
        {
            QuestEvents.DeleteQuestSteps();
            _quests = CreateQuestDictionary();
            QuestEvents.StartQuest("prologue_1");
        }
        
        private void LoadDataFromSaveFile(GameData gameData)
        {
            QuestEvents.DeleteQuestSteps();

            var loadedFlags = gameData.Flags;

            if (loadedFlags is not null)
            {
                _flagManager.LoadFlags(loadedFlags);
            }
            
            var loadedQuests = gameData.Quests;
            if (loadedQuests is not null)
            {
                foreach (var pair in loadedQuests)
                {
                    loadedQuests[pair.Key].info = Instance.Quests[pair.Key].info;
                }
                _quests = loadedQuests;
            }
            foreach (var quest in Instance.Quests.Values)
            {
                if (quest.state == QuestState.InProgress)
                {
                    QuestEvents.ChangeQuestState(quest);
                    quest.InstantiateCurrentStep(transform);
                }
                else if (quest.state == QuestState.Finished)
                {
                    quest.AddCompletedQuestDescriptions();
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

            LoadDataBetweenScenes(gameData);
        }

        private void LoadDataBetweenScenes(GameData gameData)
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
            gameData.trackedQuest = TrackedQuest.info.name;
            _flagManager.SaveFlags(gameData);
        }
    }
}


