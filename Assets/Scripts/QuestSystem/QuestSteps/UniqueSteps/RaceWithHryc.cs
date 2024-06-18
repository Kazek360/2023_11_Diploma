using System.Collections;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
using Ink.Runtime;
using UnityEngine;

public class RaceWithHryc : DialogueAreaQuestStep
{
    [SerializeField] private string delayFlag;
    private string _raceWinner;
    private bool _raceFinished;
    private bool _witFinished;
    private bool _hrycFinished;
    
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += OnAreaEnter;
        DialogueEvents.OnDialogueFinished += FinishStep;
        
        _raceFinished = false;
        _witFinished = false;
        _hrycFinished = false;
        
        area = GameObject.Find(areaToEnterName);

        StartCoroutine(StartMovement());
        
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= OnAreaEnter;
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private IEnumerator StartMovement()
    {
        if (((BoolValue) GameConfig.DialogueUI.GetVariableState(delayFlag)).value)
        {
            yield return new WaitForSeconds(1f);
        }
        PathEvents.StartMovement();
    }
    
    private void OnAreaEnter(string areaName, string objectName)
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
                StringValue winner = null;
                if (objectName.Equals("Player"))
                {
                    winner = new StringValue("wit");
                }
                else if (objectName.Equals("Hryc"))
                {
                    winner = new StringValue("hryc");
            
                }
                GameConfig.DialogueUI.SetVariableState("race_winner", winner);
                _raceFinished = true;
            }

            if (_witFinished && _hrycFinished)
            {
                AreaEvent.EnterCorrectArea(areaToEnterName);
            }
        } 
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Destroy(area);
            FinishAndDelete();
        } 
    }
}
