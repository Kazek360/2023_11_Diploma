using UnityEngine;

public class AreaToCutsceneQuestStep : QuestStep
{
    [SerializeField] private CutsceneSO CutsceneToTrigger;
    private GameObject _shoreArea;
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += FinishStep;
        _shoreArea = GameObject.Find("ShoreArea");
        _shoreArea.SetActive(false);
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= FinishStep;
    }
    private void FinishStep(string objectName)
    {
        _shoreArea.SetActive(true);
        CutsceneEvents.OpenCutscene(CutsceneToTrigger);
        Finish();
    }
}
