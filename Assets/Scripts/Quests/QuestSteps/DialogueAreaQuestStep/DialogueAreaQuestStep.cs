using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;

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
    protected override void OnDisable()
    {
        base.OnDisable();
        AreaEvent.OnAreaEnter -= AreaEntered;
    }

    protected virtual void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName))
        {
            AreaEvent.EnterCorrectArea(areaToEnterName);
        }
    }
    
    protected override void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Destroy(area);
            FinishAndDelete();
        } 
    }
}
