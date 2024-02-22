using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> _questDictionary;
    
    private void Awake()
    {
        _questDictionary = CreateQuestDictionary();
    }

    private void Start()
    {
        foreach (var quest in _questDictionary.Values)
        {
            QuestEvents.ChangeQuestState(quest);
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
        bool meetsRequirements = true;
        foreach (var questRequirement in quest.Info.requirements)
        {
            if (GetQuestById(questRequirement.id).State != QuestState.Finished)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }

    private void Update()
    {
        foreach (var quest in _questDictionary.Values)
        {
            if (quest.State == QuestState.RequirementsNotMet && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.Info.id, QuestState.CanStart);
            }
        }
    }
    
    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.State = state;
        QuestEvents.ChangeQuestState(quest);
    }
    
    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        Debug.Log($"Quest: {quest.Info.displayName} has started");
        quest.InstantiateCurrentQuestStep(transform);
        ChangeQuestState(id, QuestState.InProgress);
        
    }
    
    private void ProgressQuest(string id)
    {
        Quest quest = GetQuestById(id);
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
        Quest quest = GetQuestById(id);
        Debug.Log($"Quest: {quest.Info.displayName} has been completed");
        ChangeQuestState(id, QuestState.Finished);
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

    private Quest GetQuestById(string id)
    {
        Quest quest = _questDictionary[id];
        if (quest == null)
        {
            Debug.LogError($"There is no quest with ID: {quest.Info}");
        }

        return quest;
    }
}
