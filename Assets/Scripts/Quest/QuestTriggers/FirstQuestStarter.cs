using UnityEngine;

public class FirstQuestStarter : MonoBehaviour
{
    [SerializeField] private QuestSO quest;
    
    private void Start()
    {
        QuestEvents.StartQuest(quest.id);
        Destroy(gameObject);
    }
    
}
