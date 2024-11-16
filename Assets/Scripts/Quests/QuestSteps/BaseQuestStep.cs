using Quests;
using Quests.ScriptableObjects;
using SaveSystem;
using UnityEngine;

public abstract class BaseQuestStep : MonoBehaviour
{
    protected Quest quest;
    protected QuestStepInfo info;
    protected int progress;
    public int Progress
    {
        get => progress;
        set => progress = value;
    }

    protected virtual void OnEnable()
    {
        QuestEvents.OnQuestStepsDelete += Delete;
        progress = 0;
    }

    protected virtual void OnDestroy()
    {
        QuestEvents.OnQuestStepsDelete -= Delete;
    }
    
    public void InitializeQuestStep(Quest assignedQuest, QuestStepInfo questStepInfo)
    {
        quest = assignedQuest;
        info = questStepInfo;
    }
    
    protected void Finish()
    {
        QuestEvents.ProgressQuest(quest.info.name);
        DataPersistenceManager.Instance.SaveGame("autosave");
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
