using System.Collections;
using Dialogues;
using Quests;
using UnityEngine;

public class RaceWithHryc : DialogueAreaQuestStep
{
    [SerializeField] private string delayFlag;
    
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
        if (QuestManager.Instance.FlagManager.IsFlagTriggered(delayFlag))
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
                if (objectName.Equals("Player"))
                {
                    QuestManager.Instance.FlagManager.TriggerFlag("hryc_race_winner_player");
                }
                _raceFinished = true;
            }

            if (_witFinished && _hrycFinished)
            {
                AreaEvent.EnterCorrectArea(areaToEnterName);
            }
        } 
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
