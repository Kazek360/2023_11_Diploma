using System.Collections.Generic;
using System.Linq;
using QuestSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private List<GameObject> categoryContentParents;

    [Header("Quest Log Button")] 
    [SerializeField] private QuestLogButton questLogButtonPrefab;
    
    private Dictionary<int, Dictionary<string, QuestLogButton>> _questDictionary =
        new Dictionary<int, Dictionary<string, QuestLogButton>>();

    private void Awake()
    {
        for (var i = 0; i < categoryContentParents.Count; i++)
        {
            _questDictionary[i] = new Dictionary<string, QuestLogButton>();
        }
    }
    private int GetIndexFromQuestCategory(QuestCategory questCategory)
    {
        return questCategory switch
        {
            QuestCategory.MainQuest => 0,
            QuestCategory.SideQuest => 1,
            _ => 2
        };
    }
    public QuestLogButton GetQuestLogButton(Quest quest, UnityAction selectAction)
    {
        var index = GetIndexFromQuestCategory(quest.Info.questCategory);
        
        var questLogButton = _questDictionary[index].ContainsKey(quest.Info.name)?
            _questDictionary[index][quest.Info.name] : InstantiateQuestLogButton(quest, selectAction);
        
        return questLogButton;
    }

    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        var index = GetIndexFromQuestCategory(quest.Info.questCategory);
        
        var questLogButton =
            Instantiate(questLogButtonPrefab, categoryContentParents[index].transform).GetComponent<QuestLogButton>();
        questLogButton.GameObject().name = $"{quest.Info.name}_button";
        questLogButton.Initialize(quest.Info.displayName, selectAction);
        
        _questDictionary[index][quest.Info.name] = questLogButton;
        return questLogButton;
    }

    public QuestLogButton MoveQuestLogButtonToFinishedCategory(Quest quest, UnityAction selectAction)
    {
        var index = GetIndexFromQuestCategory(quest.Info.questCategory);
        Destroy(_questDictionary[index][quest.Info.name].gameObject);
        _questDictionary[index].Remove(quest.Info.name);
        
        var questLogButton =
            Instantiate(questLogButtonPrefab, categoryContentParents.Last().transform).GetComponent<QuestLogButton>();
        questLogButton.GameObject().name = $"{quest.Info.name}_button";
        questLogButton.Initialize(quest.Info.displayName, selectAction);
        
        _questDictionary[categoryContentParents.Count() - 1][quest.Info.name] = questLogButton;
        return questLogButton;
    }

    public void ChangeQuestsOfCategoryVisibility(int index)
    {
        foreach (var questLogButton in _questDictionary[index].Values)
        {
            questLogButton.gameObject.SetActive(!questLogButton.gameObject.activeSelf);
        }
    }
    
    public void ChangeCategoryVisibility(int index)
    {
        for (var i = 0; i < categoryContentParents.Count; i++)
        {
            if (index == i)
            {
                if (!categoryContentParents[index].gameObject.activeSelf)
                {
                    categoryContentParents[index].gameObject.SetActive(true);
                }
            }
            else
            {
                categoryContentParents[i].gameObject.SetActive(false);
            }
        }
    }
    public void MakeAllCategoriesVisible()
    {
        foreach (var category in categoryContentParents)
        {
            category.SetActive(true);
        }
    }
}
