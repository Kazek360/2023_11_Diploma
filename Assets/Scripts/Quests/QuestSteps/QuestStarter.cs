using Quests;
using UnityEngine;
using QuestSystem;

public class QuestStarter : MonoBehaviour
{
    [SerializeField] private QuestInfo quest;
    
    private void Start()
    {
        if(QuestManager.Instance.GetQuestById(quest.name).state != QuestState.InProgress && 
           QuestManager.Instance.GetQuestById(quest.name).state != QuestState.Finished)
        {
            QuestEvents.StartQuest(quest.name);
        }
        Destroy(gameObject);
    }
}
