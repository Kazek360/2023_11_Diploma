using System;
using Quests;
using SaveSystem;
using UnityEngine;

public abstract class BaseQuestStep : MonoBehaviour
{
    [TextArea][SerializeField] public string description;
    private string _questId;

    protected virtual void OnEnable()
    {
        QuestEvents.OnQuestStepsDelete += Delete;
    }


    public void InitializeQuestStep(string id)
    {
        _questId = id;
    }
    
    protected void Finish()
    {
        if (QuestManager.Instance.GetQuestById(_questId).State == QuestState.CanFinish)
        {
            QuestEvents.FinishQuest(_questId);
        }
        else
        {
            QuestEvents.ProgressQuest(_questId);
        }
        DataPersistenceManager.Instance.SaveGame("autosave");
    }

    protected void Delete()
    {
        QuestEvents.OnQuestStepsDelete -= Delete;
        Destroy(gameObject);
    }
    protected void FinishAndDelete()
    {
        Finish();
        Delete();
    }
}
