using Quests;
using UnityEngine;

public class QuestFinisher : MonoBehaviour
{
    [SerializeField] private QuestInfo quest;
    
    private void Start()
    {
        QuestEvents.FinishQuest(quest.name);
        Destroy(gameObject);
    }
}
