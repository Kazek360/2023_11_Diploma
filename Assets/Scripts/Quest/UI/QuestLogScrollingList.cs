using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private GameObject contentParent;

    [Header("Quest Log Button")] [SerializeField]
    private QuestLogButton QuestLogButtonPrefab;

    private Dictionary<string, QuestLogButton> idToButtonDictionary = new Dictionary<string, QuestLogButton>();


    public QuestLogButton CreateButtonIfNotExists(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton;
        if (!idToButtonDictionary.ContainsKey(quest.Info.id))
        {
            questLogButton = InstantiateQuestLogButton(quest, selectAction);
        }
        else
        {
            questLogButton = idToButtonDictionary[quest.Info.id];
        }

        return questLogButton;
    }
    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton =
            Instantiate(QuestLogButtonPrefab, contentParent.transform).GetComponent<QuestLogButton>();
        questLogButton.GameObject().name = $"{quest.Info.id}_button";
        questLogButton.Initialize(quest.Info.displayName, selectAction);
        idToButtonDictionary[quest.Info.id] = questLogButton;
        return questLogButton;
    }
}
