using UnityEngine;
using Dialogues;

public class DialogueAreaQuestStep : DialogueQuestStep
{
    [SerializeField] protected string areaToEnterName;
    protected GameObject area;
    protected override void OnEnable()
    {
        base.OnEnable();
        AreaEvent.OnAreaEnter += AreaEntered;
        area = GameObject.Find(areaToEnterName);
    }

    protected virtual void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName))
        {
            AreaEvent.EnterCorrectArea(areaToEnterName);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        AreaEvent.OnAreaEnter -= AreaEntered;
    }

    protected override void FinishStep(DialogueInfo dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Destroy(area);
            FinishAndDelete();
        } 
    }
}
