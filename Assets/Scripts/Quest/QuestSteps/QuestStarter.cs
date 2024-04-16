using UnityEngine;

public class QuestStarter : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    
    private void Start()
    {
        QuestEvents.StartQuest(quest.id);
        Destroy(gameObject);
    }
    
}
