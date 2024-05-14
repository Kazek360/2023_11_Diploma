using SaveSystem;
using UnityEngine;

public abstract class BaseQuestStep : MonoBehaviour
{
    [TextArea][SerializeField] public string description;
    private string _questId;

    public void InitializeQuestStep(string id)
    {
        _questId = id;
    }
    
    protected void Finish()
    {
        if (QuestManager.GetQuestById(_questId).State == QuestState.CanFinish)
        {
            QuestEvents.FinishQuest(_questId);
        }
        else
        {
            QuestEvents.ProgressQuest(_questId);
        }
        DataPersistenceManager.Instance.SaveGame();
    }

    protected void Delete()
    {
        Destroy(gameObject);
    }
    protected void FinishAndDelete()
    {
        Finish();
        Delete();
    }
}
