using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private List<GameObject> categoryContentParents;

    [Header("Quest Log Button")] 
    [SerializeField] private QuestLogButton questLogButtonPrefab;

    private Dictionary<QuestCategory, Dictionary<string, QuestLogButton>> _questDictionary =
        new Dictionary<QuestCategory, Dictionary<string, QuestLogButton>>();

    private void Awake()
    {
        QuestCategory[] questCategories = (QuestCategory[])Enum.GetValues(typeof(QuestCategory));
        foreach (var questCategory in questCategories)
        {
            _questDictionary[questCategory] = new Dictionary<string, QuestLogButton>();
        }
    }

    public QuestLogButton GetQuestLogButton(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton = _questDictionary[quest.Info.questCategory].ContainsKey(quest.Info.id)?
            _questDictionary[quest.Info.questCategory][quest.Info.id] : InstantiateQuestLogButton(quest, selectAction);
        
        return questLogButton;
    }
    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        int index;
        switch (quest.Info.questCategory)
        {
            case QuestCategory.MainQuest:
                index = 0;
                break;
            case QuestCategory.SideQuest:
                index = 1;
                break;
            default:
                index = 0;
                break;
        }
        QuestLogButton questLogButton =
            Instantiate(questLogButtonPrefab, categoryContentParents[index].transform).GetComponent<QuestLogButton>();
        questLogButton.GameObject().name = $"{quest.Info.id}_button";
        questLogButton.Initialize(quest.Info.displayName, selectAction);
        
        _questDictionary[quest.Info.questCategory][quest.Info.id] = questLogButton;
        return questLogButton;
    }

    public void ChangeQuestCategoryVisibility(int index)
    {
        QuestCategory questCategory;
        switch (index)
        {
            case 0:
                questCategory = QuestCategory.MainQuest;
                break;
            case 1:
                questCategory = QuestCategory.SideQuest;
                break;
            default:
                questCategory = QuestCategory.MainQuest;
                break;
        }
        foreach (var questLogButton in _questDictionary[questCategory].Values)
        {
            if (questLogButton.gameObject.activeSelf)
            {
                questLogButton.gameObject.SetActive(false);
            }
            else
            {
                questLogButton.gameObject.SetActive(true);
            }
        }
    }
}
