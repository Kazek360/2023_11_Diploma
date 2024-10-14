using System.Collections;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
using Ink.Runtime;
using Quests;
using UnityEngine;

public class RaceWithHryc : DialogueAreaQuestStep
{
    [SerializeField] private string delayFlag;
    private string _raceWinner;
    private bool _raceFinished;
    private bool _witFinished;
    private bool _hrycFinished;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _raceFinished = false;
        _witFinished = false;
        _hrycFinished = false;
        StartCoroutine(StartMovement());
    }
    
    private IEnumerator StartMovement()
    {
        if (QuestManager.Instance.FlagManager.GetFlagValue(delayFlag).Equals("yes"))
        {
            yield return new WaitForSeconds(1f);
        }
        PathEvents.StartMovement();
    }
    
    protected override void AreaEntered(string areaName, string objectName)
    {
        if (areaName.Equals(areaToEnterName) && (objectName.Equals("Player") || objectName.Equals("Hryc")))
        {
            if (objectName.Equals("Player"))
            {
                _witFinished = true;
            }
            else if (objectName.Equals("Hryc"))
            {
                _hrycFinished = true;
            
            }
            if (!_raceFinished)
            {
                string winner = "";
                if (objectName.Equals("Player"))
                {
                    winner = "wit";
                }
                else if (objectName.Equals("Hryc"))
                {
                    winner = "hryc";
            
                }
                QuestManager.Instance.FlagManager.SetFlagValue("race_winner", winner);
                _raceFinished = true;
            }

            if (_witFinished && _hrycFinished)
            {
                AreaEvent.EnterCorrectArea(areaToEnterName);
            }
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
