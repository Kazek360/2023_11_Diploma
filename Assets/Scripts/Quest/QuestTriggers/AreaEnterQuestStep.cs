using UnityEngine;

public class AreaEnterQuestStep : QuestStep
{
    [SerializeField] private FlagSO witMarker;
    [SerializeField] private FlagSO hrycMarker;
    private GameObject _cliffArea;
    
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += FinishStep;
        _cliffArea = GameObject.Find("CliffArea");
        _cliffArea.SetActive(false);
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
        _cliffArea.SetActive(true);
        Finish();
    }
}
