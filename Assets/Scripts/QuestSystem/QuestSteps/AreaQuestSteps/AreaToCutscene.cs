using Cutscenes;
using UnityEngine;

public class AreaToCutscene : AreaEnterQuestStep
{
    [SerializeField] private CutsceneSO CutsceneToTrigger;
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += FinishStep;
        area = GameObject.Find(areaToEnterName);
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= FinishStep;
    }
    private void FinishStep(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName))
        {
            Destroy(area);
            CutsceneEvents.StartCutscene(CutsceneToTrigger);
            Finish();
        }
    }
}
