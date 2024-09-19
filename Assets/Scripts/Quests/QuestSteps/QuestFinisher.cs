using Quests;
using UnityEngine;

public class QuestFinisher : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    
    private void Start()
    {
        QuestEvents.FinishQuest(quest.id);
        Destroy(gameObject);
    }
}
