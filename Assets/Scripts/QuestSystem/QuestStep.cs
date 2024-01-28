using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    [SerializeField] public string description;
    private bool _isFinished;
    private string _questId;
    
    private void Start()
    {
        _isFinished = false;
    }

    public void InitializeQuestStep(string id)
    {
        _questId = id;
    }
    
    protected void Finish()
    {
        if (!_isFinished)
        {
            _isFinished = true;
            QuestEvents.ProgressQuest(_questId);
            Destroy(this.gameObject);
        }
    }
}
