using Cutscenes;
using UnityEngine;

public class AreaToCutscene : AreaEnter
{
    [SerializeField] private CutsceneSO CutsceneToTrigger;
    
    protected override void FinishStep(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName))
        {
            Destroy(area);
            CutsceneEvents.StartCutscene(CutsceneToTrigger);
            FinishAndDelete();
        }
    }
}
