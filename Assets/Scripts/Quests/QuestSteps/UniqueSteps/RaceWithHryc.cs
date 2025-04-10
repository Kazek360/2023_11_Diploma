using Dialogues;
using Quests;
using Tutorials;
using UnityEngine;

public class RaceWithHryc : DialogueAreaQuestStep
{
    [SerializeField] private TutorialInfo inventoryTutorial;
    
    private bool _raceFinished;
    private bool _witFinished;
    private bool _hrycFinished;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _raceFinished = false;
        _witFinished = false;
        _hrycFinished = false;
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
                    QuestManager.Instance.FlagManager.TriggerFlag("hryc_race_win");
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
            if (QuestManager.Instance.FlagManager.IsFlagTriggered("hryc_race_reward"))
            {
                if(QuestManager.Instance.FlagManager.IsFlagTriggered("hryc_race_win"))
                {
                    GameConfig.PlayerInventory.AddItem("slingshot", 1);
                }
                else
                {
                    GameConfig.PlayerInventory.RemoveItem("knife", 1);
                }
            }
            
            TutorialManager.Instance.TriggerTutorial(inventoryTutorial);
            Destroy(area);
            FinishAndDelete();
        } 
    }
}
