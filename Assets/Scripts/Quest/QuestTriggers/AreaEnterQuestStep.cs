using UnityEngine;

public class AreaEnterQuestStep : QuestStep
{
    [SerializeField] private FlagSO witMarker;
    [SerializeField] private FlagSO hrycMarker;
    
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += FinishStep;
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= FinishStep;
    }
    private void FinishStep(string objectName)
    {
        if (objectName.Equals("Wit"))
        {
            DialogueEvents.TriggerDialogueMarker(witMarker.id);
        }
        else if (objectName.Equals("Hryc"))
        {
            DialogueEvents.TriggerDialogueMarker(hrycMarker.id);
        }
        
        Finish();
    }
}
