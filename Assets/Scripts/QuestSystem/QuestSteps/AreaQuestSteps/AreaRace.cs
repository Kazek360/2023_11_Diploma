using DialogueSystem.UI;
using Ink.Runtime;
using UnityEngine;

public class AreaRace : AreaEnterQuestStep
{
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
            StringValue winner = null;
            if (objectName.Equals("Wit"))
            {
                winner = new StringValue("wit");
            }
            else if (objectName.Equals("Hryc_Visual"))
            {
                winner = new StringValue("hryc");
            
            }
            DialogueUI.instance.SetVariableState("race_winner", winner);
            
            Finish();
        }
    }
}
