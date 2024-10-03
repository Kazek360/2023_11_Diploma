using Quests;
using SaveSystem;
using UnityEngine;

public abstract class BaseQuestStep : MonoBehaviour
{
    [TextArea] public string description;
    protected string _questId;
    [SerializeField] protected int amount;
    protected int progress;

    public int Amount => amount;
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

    public void InitializeQuestStep(string id)
    {
        _questId = id;
    }
    
    protected void Finish()
    {
        QuestEvents.ProgressQuest(_questId);
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
