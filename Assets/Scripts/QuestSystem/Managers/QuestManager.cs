using System.Collections.Generic;
using QuestSystem;
using SaveSystem;
using SaveSystem.Data;
using UnityEngine;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    public static QuestManager Instance { get; private set; }
    private static Dictionary<string, Quest> _quests;
    
    private void Awake()
    {
        if(Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (_quests is null)
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

    private void Start()
    {
        foreach (var quest in _quests.Values)
        {
            if (quest.State == QuestState.RequirementsNotMet && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.Info.id, QuestState.CanStart);
            }
        }
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
    }
    
    private void FinishQuest(string id)
    {
        var quest = GetQuestById(id);
        ChangeQuestState(id, QuestState.Finished);
        
        foreach (var q in _quests.Values)
        {
            if (q.State == QuestState.RequirementsNotMet && CheckRequirementsMet(q))
            {
                ChangeQuestState(q.Info.id, QuestState.CanStart);
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

    public static Quest GetQuestById(string id)
    {
        var quest = _quests[id];
        if (quest == null)
        {
            Debug.LogError($"There is no quest with ID: {quest.Info}");
        }
        return quest;
    }

    public void LoadData(GameData gameData)
    {
        var loadedQuests = gameData.quests;
        if (loadedQuests is not null)
        {
            foreach (var pair in loadedQuests)
            {
                loadedQuests[pair.Key].Info = _quests[pair.Key].Info;
            }
            _quests = loadedQuests;
        }

        foreach (var quest in _quests.Values)
        {
            if (quest.State == QuestState.InProgress)
            {
                QuestEvents.ChangeQuestState(quest);
                quest.InstantiateCurrentQuestStep(transform);
            }
            else if(quest.State == QuestState.Finished)
            {
                QuestEvents.ChangeQuestState(quest);
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.quests = _quests;
    }
}
