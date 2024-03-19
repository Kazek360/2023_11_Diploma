using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GameObject contentParent;

    [Header("Quest Log Button")] 
    [SerializeField] private QuestLogButton questLogButtonPrefab;

    private Dictionary<string, QuestLogButton> _idToButtonDictionary = new Dictionary<string, QuestLogButton>();


    public QuestLogButton GetQuestLogButton(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton = _idToButtonDictionary.ContainsKey(quest.Info.id)? 
            _idToButtonDictionary[quest.Info.id] : InstantiateQuestLogButton(quest, selectAction);
        return questLogButton;
    }
    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton =
            Instantiate(questLogButtonPrefab, contentParent.transform).GetComponent<QuestLogButton>();
        questLogButton.GameObject().name = $"{quest.Info.id}_button";
        questLogButton.Initialize(quest.Info.displayName, selectAction);
        _idToButtonDictionary[quest.Info.id] = questLogButton;
        return questLogButton;
    }
}
