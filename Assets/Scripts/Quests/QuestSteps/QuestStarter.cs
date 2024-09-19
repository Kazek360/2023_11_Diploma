using Quests;
using UnityEngine;
using QuestSystem;

public class QuestStarter : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    
    private void Start()
    {
        if(QuestManager.Instance.GetQuestById(quest.id).State != QuestState.InProgress && 
           QuestManager.Instance.GetQuestById(quest.id).State != QuestState.Finished)
        {
            QuestEvents.StartQuest(quest.id);
        }
        Destroy(gameObject);
    }
}
