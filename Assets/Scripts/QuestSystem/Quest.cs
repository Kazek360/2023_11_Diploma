using UnityEngine;

public class Quest
{
    public QuestSO Info;
    public QuestState State;
    private int _currentStepIndex;

    public Quest(QuestSO info)
    {
        Info = info;
        State = QuestState.RequirementsNotMet;
        _currentStepIndex = 0;
    }

    public bool CurrentStepExists()
    {
        return _currentStepIndex < Info.steps.Count;
    }
    
    public bool NextStepExists()
    {
        return _currentStepIndex + 1 < Info.steps.Count;
    }
    
    public void MoveToNextStep()
    {
        _currentStepIndex++;
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        if (CurrentStepExists())
        {
            QuestStep questStep = Object.Instantiate(Info.steps[_currentStepIndex], parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(Info.id);
            Debug.Log($"Current task: {questStep.description}");
        }
        else
        {
            Debug.LogWarning($"Quest with id: {Info.id} does not have step: {_currentStepIndex}");
        }
    }
}
