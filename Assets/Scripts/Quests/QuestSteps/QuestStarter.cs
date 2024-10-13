using Quests;
using UnityEngine;
using QuestSystem;

public class QuestStarter : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    
    private void Start()
    {
        if(QuestManager.Instance.GetQuestById(quest.name).State != QuestState.InProgress && 
           QuestManager.Instance.GetQuestById(quest.name).State != QuestState.Finished)
        {
            QuestEvents.StartQuest(quest.name);
        }
        Destroy(gameObject);
    }
}
