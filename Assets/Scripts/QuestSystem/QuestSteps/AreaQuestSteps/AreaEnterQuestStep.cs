using UnityEngine;

public class AreaEnterQuestStep : BaseQuestStep
{
    [SerializeField] protected string areaToEnterName;
    protected GameObject area;
    
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
            FinishAndDelete();
        }
    }
}
