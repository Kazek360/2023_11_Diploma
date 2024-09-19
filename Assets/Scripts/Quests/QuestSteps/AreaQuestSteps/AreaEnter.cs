using UnityEngine;

public class AreaEnter : BaseQuestStep
{
    [SerializeField] protected string areaToEnterName;
    protected GameObject area;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        AreaEvent.OnAreaEnter += FinishStep;
        area = GameObject.Find(areaToEnterName);
    }
    protected void OnDisable()
    {
        AreaEvent.OnAreaEnter -= FinishStep;
    }
    protected virtual void FinishStep(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName))
        {
            Destroy(area);
            FinishAndDelete();
        }
    }
}
