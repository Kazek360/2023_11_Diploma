using UnityEngine;

public class QuestStarter : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    
    private void Start()
    {
        if(QuestManager.GetQuestById(quest.id).State != QuestState.InProgress && 
           QuestManager.GetQuestById(quest.id).State != QuestState.Finished)
        {
            QuestEvents.StartQuest(quest.id);
        }
        Destroy(gameObject);
    }
    
}
