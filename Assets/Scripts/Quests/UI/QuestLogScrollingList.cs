using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Quests.UI
{
    public class QuestLogScrollingList : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private List<GameObject> categoryContentParents;
        [SerializeField] private List<GameObject> categoryText;
        [SerializeField] private List<GameObject> categoryTabImage;
        [SerializeField] private List<GameObject> categoryTabButton;
        
        [Header("Quest Log Button")] 
        [SerializeField] private QuestLogButton questLogButtonPrefab;

        private Dictionary<int, Dictionary<string, QuestLogButton>> _questDictionary = new();

        private void Awake()
        {
            for (var i = 0; i < categoryContentParents.Count; i++)
            {
                _questDictionary[i] = new Dictionary<string, QuestLogButton>();
            }
            ChangeCategoryVisibility(0);
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
            var index = GetIndexFromQuestCategory(quest.info.questCategory);

            var questLogButton = _questDictionary[index].ContainsKey(quest.info.name)
                ? _questDictionary[index][quest.info.name]
                : InstantiateQuestLogButton(quest, selectAction);

            return questLogButton;
        }

        private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
        {
            var index = GetIndexFromQuestCategory(quest.info.questCategory);

            var questLogButton =
                Instantiate(questLogButtonPrefab, categoryContentParents[index].transform)
                    .GetComponent<QuestLogButton>();
            questLogButton.gameObject.name = $"{quest.info.name}_button";
            questLogButton.Initialize(quest.info.displayName, selectAction);

            _questDictionary[index][quest.info.name] = questLogButton;
            return questLogButton;
        }

        public QuestLogButton MoveQuestLogButtonToFinishedCategory(Quest quest, UnityAction selectAction)
        {
            var index = GetIndexFromQuestCategory(quest.info.questCategory);
            if (_questDictionary[index].ContainsKey(quest.info.name))
            {
                Destroy(_questDictionary[index][quest.info.name].gameObject);
                _questDictionary[index].Remove(quest.info.name); 
            }
            
            var questLogButton =
                Instantiate(questLogButtonPrefab, categoryContentParents.Last().transform)
                    .GetComponent<QuestLogButton>();
            questLogButton.gameObject.name = $"{quest.info.name}_button";
            questLogButton.Initialize(quest.info.displayName, selectAction);

            _questDictionary[categoryContentParents.Count - 1][quest.info.name] = questLogButton;
            return questLogButton;
        }

        public void ChangeCategoryVisibility(int index)
        {
            for (var i = 0; i < categoryContentParents.Count; i++)
            {
                categoryContentParents[i].gameObject.SetActive(index == i);
                categoryText[i].gameObject.SetActive(index == i);
                categoryTabImage[i].gameObject.SetActive(index == i);
                categoryTabButton[i].gameObject.SetActive(index != i);
            }
        }
    }
}
