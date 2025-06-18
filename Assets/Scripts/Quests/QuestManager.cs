using System.Collections.Generic;
using Quests.Flags;
using Quests.ScriptableObjects;
using SaveSystem;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using Shared;
using UnityEngine;

namespace Quests
{
    public class QuestManager : Singleton<QuestManager>, INewGameInitializable, IFromFileLoadable, IBetweenScenesLoadable
    {
        [Header("First Quest")]
        [SerializeField] private string questName;
        
        [Header("Load Globals JSON")] 
        [SerializeField] private TextAsset loadGlobals;
        
        [SerializeField] private bool debugNoInit;
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
            SaveSystemEvents.OnNewGame += InitializeOnNewGame;
            SaveSystemEvents.OnFromFileLoad += LoadDataFromSaveFile;
            SaveSystemEvents.OnBetweenScenesLoad += LoadDataBetweenScenes;
            SaveSystemEvents.OnSave += SaveData;
            
            QuestEvents.OnQuestStart += StartQuest;
            QuestEvents.OnQuestProgress += ProgressQuest;
            QuestEvents.OnQuestStepProgress += UpdateQuestStepProgress;
        }

        private void OnDisable()
        {
            SaveSystemEvents.OnNewGame -= InitializeOnNewGame;
            SaveSystemEvents.OnFromFileLoad -= LoadDataFromSaveFile;
            SaveSystemEvents.OnBetweenScenesLoad -= LoadDataBetweenScenes;
            SaveSystemEvents.OnSave -= SaveData;
            
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

        private void StartQuest(string id)
        {
            var quest = GetQuestById(id);
            quest.InstantiateFirstStep(transform);
            quest.state = QuestState.InProgress;
            QuestEvents.StartQuest(quest);
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
                GameConfig.PlayerInventory.AddItem(item.item.name, item.amount);
            }

            quest.state = QuestState.Finished;
            QuestEvents.FinishQuest(quest);

            foreach (var transition in quest.info.nextQuestsTransitions)
            {
                if (CheckForRequiredFlagsState(transition.requiredFlags))
                {
                    var nextQuestInfo = transition.nextQuest;
                    
                    if (nextQuestInfo.questCategory == QuestCategory.MainQuest)
                    {
                        QuestEvents.StartQuest(nextQuestInfo.name);
                    }
                    else
                    {
                        var nextQuest = GetQuestById(nextQuestInfo.name);
                        nextQuest.state = QuestState.CanStart;
                    }
                }
            }

            if (TrackedQuest.state == QuestState.Finished)
            {
                TrackedQuest = null;
                QuestEvents.StopTrackingQuest();
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
        
        public void InitializeOnNewGame()
        {
            if (!debugNoInit)
            {
                QuestEvents.DeleteQuestSteps();
                _quests = CreateQuestDictionary();
                QuestEvents.StartQuest(questName);
            }
        }
        
        public void LoadDataFromSaveFile(GameData gameData)
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
                    quest.InstantiateCurrentStep(transform);
                }
                else if (quest.state == QuestState.Finished)
                {
                    quest.PassThroughCompletedSteps();
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

        public void LoadDataBetweenScenes(GameData gameData)
        {
            foreach (var quest in Instance.Quests.Values)
            {
                QuestEvents.AddQuestToLog(quest);
            }
            QuestEvents.TrackQuest();
        }

        public void SaveData(GameData gameData)
        {
            gameData.Quests = _quests;
            gameData.trackedQuest = TrackedQuest.info.name;
            _flagManager.SaveFlags(gameData);
        }
    }
}


