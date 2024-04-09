using UnityEngine;

public class AreaToCutsceneQuestStep : QuestStep
{
    [SerializeField] private CutsceneSO CutsceneToTrigger;
    private GameObject _shoreToTeleport;
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += FinishStep;
        _shoreToTeleport = GameObject.Find("ShoreArea");
        _shoreToTeleport.SetActive(false);
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= FinishStep;
    }
    private void FinishStep()
    {
        _shoreToTeleport.SetActive(true);
        CutsceneEvents.OpenCutscene(CutsceneToTrigger);
        Finish();
    }
}
