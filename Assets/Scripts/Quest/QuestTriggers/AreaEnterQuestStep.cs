using System.Collections.Generic;
using UnityEngine;

public class AreaEnterQuestStep : QuestStep
{
    private GameObject _cliffArea;
    [SerializeField] private List<string> areasToDisableNames;
    private List<GameObject> areasToDisable = new List<GameObject>();
    
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += FinishStep;
        for (var i = 0; i < areasToDisableNames.Count; i++)
        {
            areasToDisable.Add(GameObject.Find(areasToDisableNames[i]));
            areasToDisable[i].SetActive(false);
        }
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= FinishStep;
    }
    private void FinishStep(string objectName)
    {
        for (var i = 0; i < areasToDisableNames.Count; i++)
        {
            areasToDisable[i].SetActive(true);
        }
        Finish();
    }
}
