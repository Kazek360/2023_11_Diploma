using System.Collections.Generic;
using DialogueSystem.UI;
using Ink.Runtime;
using UnityEngine;

public class AreaEnterQuestStep : QuestStep
{
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
        for (var i = 0; i < areasToDisableNames.Count; i++)
        {
            areasToDisable[i].SetActive(true);
        }
        Finish();
    }
}
